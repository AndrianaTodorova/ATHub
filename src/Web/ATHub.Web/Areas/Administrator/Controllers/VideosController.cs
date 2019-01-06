using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using ATHub.Web.Areas.Videos.Models;
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
          
           var model =  this.videoService.GetEditVideoData(id);
            return new JsonResult(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditVideo(VideoEditModel model)
        {
            //TODO update video in database! May be it will better if you check for changes before save(update) the object :)
            
            var video = await this.videoService.EditVideo(model.Id,model.Name, model.Link, model.Description, model.Category);
            return this.RedirectToAction("ManageVideos", "Videos", new { area = "Administrator" });
        }
    }
}
