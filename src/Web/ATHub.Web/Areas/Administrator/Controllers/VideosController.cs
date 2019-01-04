using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class VideosController : Controller
    {
        public readonly IVideoService videoService;

        public VideosController(IVideoService videoService)
        {
            this.videoService = videoService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageVideos()
        {
            var model = new AdminAllVideosModel() { Videos = this.videoService.GetAdminVideoModel() };
            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deled = await this.videoService.DeleteVideo(id);
            return this.RedirectToAction("ManageVideos", "Videos", new { area="Administrator"});
        }

        public JsonResult EditVideo(int id)
        {
            var a = new Dictionary<int, string>();
            a.Add(id, "tuk");
           var model =  this.videoService.GetEditVideoData(id);
            return new JsonResult(model);
        }
     

    }
}
