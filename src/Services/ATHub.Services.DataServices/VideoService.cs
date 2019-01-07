using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                UploadDate = DateTime.UtcNow,
             
            };

           await this.videoRepository.AddAsync(video);
           await this.videoRepository.SaveChangesAsync();

            return video.Id;
        }

        public DetailsVideoModel GetDetailsVideoModel(int id)
        {
            this.UpdateViews(id);
            var model = this.videoRepository.All().Where(p => p.Id == id && p.DeletedOn == null).Select(x => new DetailsVideoModel()
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
            var videos = this.videoRepository.All().Where(v => v.DeletedOn == null)
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
               .Where(x => x.Name.Contains(search) && x.DeletedOn == null)
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

        public IList<SingleAdminVideoModel> GetAdminVideoModel()
        {
            
            var model = this.videoRepository.All().Select(v => new SingleAdminVideoModel()
            {
                Id = v.Id,
                Name = v.Name,
                UploadedOn = v.UploadDate.ToShortDateString(),
                Author = v.Uploader.UserName,
                DeletedOn = v.DeletedOn.HasValue ? v.DeletedOn.Value.ToString("yyyy-MM-dd") : null
            }).ToList();

            return model;
        }

        public EditAdminVideoViewModel GetEditVideoData(int id)
        {
            var model = this.videoRepository.All().Where(x => x.Id == id).Select(s => new EditAdminVideoViewModel()
            {
                Link = s.Link,
                Name = s.Name,
                Description = s.Description,
                Category = s.Category.Name,
                categoryNames = this.categoryRepository.All().Where(x => x.DeletedOn == null).Select(c => c.Name).ToHashSet()
            }).FirstOrDefault();
            return model;
        }
        public async Task<int> DeleteVideo(int id)
        {
            var currentVideo = this.videoRepository.All().FirstOrDefault(v => v.Id == id);
            if(currentVideo == null)
            {
                throw new ArgumentException("Invalid id");
            }
            if(currentVideo.DeletedOn == null)
            {
                currentVideo.DeletedOn = DateTime.UtcNow;
            }
            else
            {
                currentVideo.DeletedOn = null;
            }
          
            await this.videoRepository.SaveChangesAsync();
            return id;
        }

        public async Task<int> EditVideo(int id,string name, string link, string desc, string category)
        {
            var video = this.videoRepository.All().FirstOrDefault(v => v.Id == id);
            if(video == null)
            {
                throw new ArgumentException($"Video with id {id} does not exist");
            }
            video.Name = name;
            video.Description = desc;
            video.Link = link;
            var videoCategory = this.categoryRepository.All().FirstOrDefault(c => c.Name == category && c.DeletedOn == null);
            if(videoCategory == null)
            {
                //TODO
            }
            video.Category = videoCategory;
            videoCategory.Videos.Add(video);
            
            await videoRepository.SaveChangesAsync();
            return video.Id;
        }

        public TrendingViewModel GetTrendingVideoModel()
        {
            var model = new TrendingViewModel();
            var categories = this.categoryRepository.All().Select(c =>
            new CategoriesViewModel()
            {
                Name = c.Name,
                Videos = c.Videos.OrderByDescending(p => p.Views).Take(6).Select(v => new VideoModel() { Id = v.Id, Link = this.GetEmbed(v.Link), Title = v.Name }).ToList()
            }).OrderBy(n => n.Name).ToList();
            model.Categories = categories;
            return model;
        }
    }
}
