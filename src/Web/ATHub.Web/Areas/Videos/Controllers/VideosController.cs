using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Web.Areas.Videos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATHub.Web.Areas.Videos.Controllers
{
    
    public class VideosController : Controller
    {
        private readonly IRepository<Video> videoRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly UserManager<ATHubUser> _manager;

        public VideosController(IRepository<Video> videoRepository, 
            IRepository<Category> categoryRepository
            , UserManager<ATHubUser> _manager) 
        {                                                          
            this.videoRepository = videoRepository;
            this._manager = _manager;
            this.categoryRepository = categoryRepository;
        }

        [Area("Videos")]
        public IActionResult Create()
        {
            return View();
        }

      
        public IActionResult Create(VideoCreateModel model)
        {
            var uploader =  _manager.GetUserAsync(HttpContext.User).Result;
            var video = new Video()
            {
                Name = model.Name,
                Description = model.Description,
                Link = model.Link,
                Category = categoryRepository.All().FirstOrDefault(c => c.Name == model.Category),
                Uploader = uploader,
                UploadDate = DateTime.UtcNow
            };

            this.videoRepository.AddAsync(video);
            this.videoRepository.SaveChangesAsync();
            return this.RedirectToAction("/Index");
        }
    }
}