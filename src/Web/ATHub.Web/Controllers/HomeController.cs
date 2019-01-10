using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ATHub.Web.Models;
using ATHub.Web.Models.Home;
using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.DataServices;
using ATHub.Services.Data.Models;
using Microsoft.AspNetCore.Http.Features;
using System;

namespace ATHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Video> videoRepository;
        public readonly IVideoService videoService;

        public HomeController(IRepository<Video> videoRepository, IVideoService videoService)
        {
            this.videoRepository = videoRepository;
            this.videoService = videoService;
        }

        
        public IActionResult Index()
        {

            var videoModel = this.videoService.GetRandomVideos(10);
            var models = new IndexViewModel()
            {
                Videos = videoModel
            };
            return View(models);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpPost]
        public IActionResult Search(string searchParam)
        {

            var searched = this.videoService.SearchVideos(searchParam).ToList();
            if (searched.Count() == 0)
            {
                return View("NotFound");
            }
            var models = new IndexViewModel()
            {
                Videos = searched
            };
            return View(models);
        }

      
        public JsonResult GetSearchValue(string searchParam)
        {
            List<SearchModel> modelList = this.videoRepository
                .All()
                .Where(x => x.Name.Contains(searchParam) && x.DeletedOn == null)
                .Select(v => new SearchModel()
                {
                    Id = v.Id,
                    Name = v.Name
                })
                .ToList();
          

            return new JsonResult(modelList);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult Error(int statusCode)
        {
            HashSet<int> statuses = new HashSet<int>() { 400, 401, 403, 404, 500 };

            int code = HttpContext.Features.Get<IHttpResponseFeature>().StatusCode;

            if( code > 300 || statuses.Contains(statusCode))
            {       
                return View("Error-" + Math.Max(statusCode,code));
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
      
    }
}
