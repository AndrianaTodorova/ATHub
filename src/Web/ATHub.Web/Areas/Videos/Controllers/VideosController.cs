using System.Linq;
using System.Threading.Tasks;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using ATHub.Web.Areas.Videos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ATHub.Web.Areas.Videos.Controllers
{

    [Area("videos")]
    public class VideosController : Controller
    {
        public readonly IVideoService videoService;
        public readonly ICategoryService categoryService;
        private readonly UserManager<ATHubUser> _manager;

        public VideosController(IVideoService videoService
            , UserManager<ATHubUser> _manager,
            ICategoryService categoryService)
        {
            this.videoService = videoService;
            this._manager = _manager;
            this.categoryService = categoryService;

        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Trending()
        {
            var model = this.videoService.GetTrendingVideoModel();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                //TODO return this.RedirectToAction("Log in", new { area = "Identity"});
            }
            
            var detailsModel = this.videoService.GetDetailsVideoModel(id);
           
            return View(detailsModel);
        }

        public JsonResult Comments(int id)
        {
            var comments = this.videoService.GetComments(id);
            return new JsonResult(comments);
        }
        [HttpPost]
        [Authorize]
        public  async Task<IActionResult> Create(VideoCreateModel model)
        {
            var uploader = _manager.GetUserAsync(HttpContext.User).Result;
            int id = await videoService
                .Create(model.Name, model.Description, model.Link, model.Category, uploader);
            return this.RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}