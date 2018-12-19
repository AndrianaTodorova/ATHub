using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;

namespace ATHub.Services.DataServices
{
    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> commentsRepository;
        private readonly IRepository<Video> videoRepository;

        public CommentsService(IRepository<Comment> commentsRepository,
           IRepository<Video> videoRepository)
        {
            this.commentsRepository = commentsRepository;
            this.videoRepository = videoRepository;
        }
        public async Task<int> Add(string content, ATHubUser uploader, int id)
        {
            var comment = new Comment()
            {
                Text = content,
                WrittenDate = DateTime.UtcNow,
                Author = uploader,
                AuthorId = uploader.Id,
                
            };

            
            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
            var video = this.videoRepository.All().FirstOrDefault(x => x.Id == id);
            video.Comments.Add(comment);
            await this.videoRepository.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<int> Delete(int id)
        {
            var comment = this.commentsRepository.All().FirstOrDefault(x => x.Id == id);
            this.commentsRepository.Delete(comment);
            await this.commentsRepository.SaveChangesAsync();
            return comment.Id;
        }
      
        public async Task<int> Edit(string content,int id)
        {
            var comment = this.commentsRepository.All().FirstOrDefault(c => c.Id == id);
            comment.Text = content;
            await this.commentsRepository.SaveChangesAsync();
            return comment.Id;
        }
    }
}
