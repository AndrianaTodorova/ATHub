using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ATHub.Tets
{
    public class VideoServiceTests : BaseServiceTests
    {
        private const string Miley = "Miley Cyrus";
        private const string Link1 = "https://www.youtube.com/watch?v=Cl2kZmGVbyY&start_radio=1&list=RDCl2kZmGVbyY";

        private IVideoService VideoServiceMock;
        public VideoServiceTests()
        {
            this.VideoServiceMock = this.ServiceProvider.GetRequiredService<IVideoService>();

        }

        [Test]
        public async Task CreateVideo()
        {
           
           
            await this.DbContext.SaveChangesAsync();

            var expected = new Video
            { Id = 1, Name = Miley, Description = "desc", Link = Link1, Category = new Category() { Id = 1, Name = "Category1", CreatedOn = DateTime.UtcNow }, UploadDate = DateTime.UtcNow, Uploader = uploader };

            var actual = await this.VideoServiceMock.Create(Miley, "desc" ,Link1, "Category1", uploader);
            Assert.That(expected.Id.Equals(actual));
        }
        [Test]
        public void GetDetailsVideoModel()
        {
           
          this.VideoServiceMock.Create(Miley, "desc", Link1, "Category1", uploader).Wait();
          var a =  this.DbContext.Videos.Where(v => v.Name == Miley).FirstOrDefault();
            var expected = this.DbContext.Videos.Where(p => p.Id == a.Id && p.DeletedOn == null).Select(x => new DetailsVideoModel()
            {
                Id = x.Id,
                Title = x.Name,
                Description = x.Description,
                UploaderName = x.Uploader.UserName,
                Views = x.Views,
                Link = this.GetEmbed(x.Link),
                UploadDate = x.UploadDate.ToShortDateString(),
            }).FirstOrDefault();

            var actual = this.VideoServiceMock.GetDetailsVideoModel(a.Id);
           
            Assert.That(expected.Title.Equals(actual.Title));
            Assert.That(expected.UploaderName.Equals(actual.UploaderName));
            Assert.That(expected.Link.Equals(actual.Link));
        }

        [Test]
        
        public void GetComments()
        {
            
            var comments = new List<Comment>()
            {
                new Comment(){Text = "first comment", Author = uploader, WrittenDate = DateTime.UtcNow},
                 new Comment(){Text = "second comment", Author = uploader, WrittenDate = DateTime.UtcNow}
            };

            this.DbContext.Comments.AddRangeAsync(comments).Wait();
             this.DbContext.SaveChangesAsync().Wait();

            var expected = new Video
            { Name = Miley, Description = "desc", Link = Link1, Category = new Category() {  Name = "Category1", CreatedOn = DateTime.UtcNow }, UploadDate = DateTime.UtcNow, Uploader = uploader, Comments =  comments};

            this.DbContext.Videos.AddAsync(expected).Wait();
             this.DbContext.SaveChangesAsync().Wait();

            var actual = this.VideoServiceMock.GetComments(expected.Id);
            Assert.That(expected.Comments.Count.Equals(actual.Count()));
   
            Assert.That(expected.Comments.OrderBy(p => p.Text).FirstOrDefault().Text.Equals(actual.OrderBy(p => p.Text).FirstOrDefault().Text));

        }

        [Test]
        public async Task SearchVideos()
        {
            
            await this.VideoServiceMock.Create(Miley, "desc", Link1, "Category1", uploader);
            await this.VideoServiceMock.Create(Miley + "Second", "desc", Link1, "Category1", uploader);
            var searched = "Miley";
            var expected = this.DbContext.Videos
               .Where(x => x.Name.Contains(searched) && x.DeletedOn == null)
               .Select(x =>
             new VideoModel()
             {
                 Id = x.Id,
                 Link = this.GetEmbed(x.Link),
                 Title = x.Name
             })
             .ToList();
            var actual = this.VideoServiceMock.SearchVideos(searched);
            Assert.That(expected.Count.Equals(actual.Count()));


        }

        [Test]
        public async Task DeleteVideo()
        {
            
            var expected = new Video
            { Name = "Some name", Description = "desc", Link = Link1, Category = new Category() {  Name = "Some name", CreatedOn = DateTime.UtcNow }, UploadDate = DateTime.UtcNow, Uploader = uploader };

            await this.DbContext.AddAsync(expected);
            await this.DbContext.SaveChangesAsync();
            var videoId = this.DbContext.Videos.FirstOrDefault(v => v.Name == "Some name");
            var actual = this.VideoServiceMock.DeleteVideo(videoId.Id);

            Assert.That(this.DbContext.Videos.Find(videoId.Id).DeletedOn.HasValue.Equals(true));
        }

        [Test]
        public void EditVideo()
        {
            
            var video = new Video
            {  Name = Miley, Description = "desc", Link = Link1, Category = new Category() { Name = "Category1", CreatedOn = DateTime.UtcNow }, UploadDate = DateTime.UtcNow, Uploader = uploader };

             this.DbContext.AddAsync(video);
                this.DbContext.SaveChangesAsync();

            var newDescription = "new description";
            var newName = "newName";
            var videoId = this.DbContext.Videos.FirstOrDefault(v => v.Name == Miley);
            var actual = this.VideoServiceMock.EditVideo(videoId.Id, newName, Link1, newDescription, "Category1");

            Assert.That(this.DbContext.Videos.Find(videoId.Id).Description.Equals(newDescription));
            Assert.That(this.DbContext.Videos.Find(videoId.Id).Name.Equals(newName));
        }

        [Test]
        public void DeleteVideoWithInvalidIdThrowsException()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(() =>
               this.VideoServiceMock.DeleteVideo(4));
            Assert.That("Invalid id".Equals(exception.Message));
        }


        [Test]
        public void EditVideoWithInvalidIdThrowsException()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(() =>
               this.VideoServiceMock.EditVideo(4, "test", "test", "test", "test"));
            Assert.That("Video with id 4 does not exist".Equals(exception.Message));
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
    }
}
