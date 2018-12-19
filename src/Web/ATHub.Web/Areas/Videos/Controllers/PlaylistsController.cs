using ATHub.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Videos.Controllers
{
    [Area("videos")]
    public class PlaylistsController : Controller
    {
        public readonly IVideoService videoService;
        public readonly IPlaylistService playlistService;

        private readonly UserManager<ATHubUser> _manager;


        public PlaylistsController(IVideoService videoService, 
            IPlaylistService playlistService,
            UserManager<ATHubUser> _manager)
        {
            this.playlistService = playlistService;
            this.videoService = videoService;
            this._manager = _manager;
        }

        public IActionResult MyPlaylist()
        {
            var username = this.User.Identity.Name;
            var model = this.playlistService.GetPlaylistModel(username);
            return this.View(model);
        }
        public async Task<IActionResult> AddToPlaylist(int id)
        {
            var currenUser = this._manager.GetUserAsync(HttpContext.User).Result;
            int videoPlaylistId = await this.playlistService.AddToPlaylist(id, currenUser);

            return  this.Redirect("MyPlaylist");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var currenUser = this._manager.GetUserAsync(HttpContext.User).Result;
            await this.playlistService.Remove(id, currenUser);
            return this.Redirect("MyPlaylist");
        } 
    }
}
