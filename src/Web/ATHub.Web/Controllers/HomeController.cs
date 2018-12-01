using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ATHub.Web.Models;
using ATHub.Web.Models.Home;
using ATHub.Data.Common;
using ATHub.Data.Models;
using System.Web;

namespace ATHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Video> videoRepository;

        public HomeController(IRepository<Video> videoRepository)
        {
            this.videoRepository = videoRepository;
        }
        public IActionResult Index()
        {

            var videoModel = this.videoRepository.All().OrderBy(x => Guid.NewGuid()).Select(x =>
             new VideoModel()
             {
                 Link = this.GetEmbed(x.Link),
                 Title = x.Name
             }).Take(12).ToList();

            var models = new VideoModels()
            {
                VideoModelss = videoModel
            };
            return View(models);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private string GetEmbed(string link)
        {

            var uri = new Uri(link);

            // you can check host here => uri.Host <= "www.youtube.com"

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
