using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ATHub.Tets
{
    public class VideoServiceTests : BaseServiceTests
    {
        private const string Miley = "Miley Cyrus";
        private const string Link1 = "https://www.youtube.com/watch?v=Cl2kZmGVbyY&start_radio=1&list=RDCl2kZmGVbyY";

        private IVideoService VideoServiceMock => this.ServiceProvider.GetRequiredService<IVideoService>();

        [Test]
        public async Task CreateVideo()
        {
            var uploader = this.AddTestingUserToDb();
           
            await this.DbContext.SaveChangesAsync();

            var expected = new Video
            { Id = 1, Name = Miley, Description = "desc", Link = Link1, Category = new Category() { Id = 1, Name = "Category1", CreatedOn = DateTime.UtcNow }, UploadDate = DateTime.UtcNow, Uploader = uploader };

            var actual = await this.VideoServiceMock.Create(Miley, "desc" ,Link1, "Category1", uploader);
            Assert.That(expected.Id.Equals(actual));
        }
        [Test]
        public async Task GetDetailsVideoModel()
        {
            var uploader = this.AddTestingUserToDb();
           await this.VideoServiceMock.Create(Miley, "desc", Link1, "Category1", uploader);
            var id = 1;
            var expected = this.DbContext.Videos.Where(p => p.Id == id && p.DeletedOn == null).Select(x => new DetailsVideoModel()
            {
                Id = x.Id,
                Title = x.Name,
                Description = x.Description,
                UploaderName = x.Uploader.UserName,
                Views = x.Views,
                Link = this.GetEmbed(x.Link),
                UploadDate = x.UploadDate.ToShortDateString(),
            }).FirstOrDefault();

            var actual = this.VideoServiceMock.GetDetailsVideoModel(id);
           
            Assert.That(expected.Title.Equals(actual.Title));
            Assert.That(expected.UploaderName.Equals(actual.UploaderName));
            Assert.That(expected.Link.Equals(actual.Link));
        }

        [Test]
        public async Task SearchVideos()
        {
            var uploader = this.AddTestingUserToDb();
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
        private ATHubUser AddTestingUserToDb()
        {
            var result = new ATHubUser
            {
                Id = "1",
                UserName = "testName",
                Email = "test@mail.bg",
            };
            this.DbContext.Users.Add(result);
            this.DbContext.SaveChangesAsync();
            return result;
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
