using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ATHub.Services.DataServices
{
    public class VideoService : IVideoService
    {
        private readonly IRepository<Video> videoRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly UserManager<ATHubUser> _manager;

        public VideoService(IRepository<Video> videoRepository,
          IRepository<Category> categoryRepository
          , UserManager<ATHubUser> _manager)
        {
            this.videoRepository = videoRepository;
            this._manager = _manager;
            this.categoryRepository = categoryRepository;
        }

        public async Task<int> Create(string name, string description, string link, string category, ATHubUser uploader)
        {
            
            var video = new Video()
            {
                Name = name,
                Description = description,
                Link = link,
                Category = categoryRepository.All().FirstOrDefault(c => c.Name == category),
                Uploader = uploader,
                UploadDate = DateTime.UtcNow
            };

           await this.videoRepository.AddAsync(video);
           await this.videoRepository.SaveChangesAsync();

            return video.Id;
        }

        public DetailsVideoModel GetDetailsVideoModel(int id)
        {
            if (id==3)
            {
                throw new ArgumentException("Fuck off");
            }
            this.UpdateViews(id);
            var model = this.videoRepository.All().Where(p => p.Id == id).Select(x => new DetailsVideoModel()
            {
                Id = x.Id,
                Title = x.Name,
                Description = x.Description,
                UploaderName = x.Uploader.UserName,
                Views = x.Views,
                Link = this.GetEmbed(x.Link),
                UploadDate = x.UploadDate.ToShortDateString(), 
            }).FirstOrDefault();

            return model;
        }
        public IEnumerable<CommentsDetailsVideoModel> GetComments(int id)
        {
            var model = this.videoRepository.All().FirstOrDefault(v => v.Id == id);
            var comments = model.Comments.OrderByDescending(c => c.WrittenDate).Select(c => new CommentsDetailsVideoModel() { Id = c.Id, Text = c.Text, Date = c.WrittenDate.ToShortDateString(), UploaderName = c.Author.UserName });

            return comments;
        }
        public IEnumerable<VideoModel> GetRandomVideos(int count)
        {
            var videos = this.videoRepository.All()
                .OrderBy(x => Guid.NewGuid())
               .Select(x =>
             new VideoModel()
             {
                 Id = x.Id,
                 Link = this.GetEmbed(x.Link),
                 Title = x.Name
             }).Take(12).ToList();

            return videos;
        }

        public IEnumerable<VideoModel> SearchVideos(string search)
        {
            var videos = this.videoRepository.All()
               .Where(x => x.Name.Contains(search))
               .Select(x =>
             new VideoModel()
             {
                 Id = x.Id,
                 Link = this.GetEmbed(x.Link),
                 Title = x.Name
             })
             .ToList();

            return videos;
        }

        private string GetEmbed(string link)
        {

            var uri = new Uri(link);

            var query = HttpUtility.ParseQueryString(uri.Query);

            var videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }
            return videoId;
        }

        private void UpdateViews(int id)
        {
            var video = this.videoRepository.All().FirstOrDefault(x => x.Id == id);
            if(video != null)
            {
                video.Views++;
                this.videoRepository.SaveChangesAsync();
            }
            //TODO not found exception
        
        }

    }
}
