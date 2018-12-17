using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Videos.Controllers
{
    [Area("videos")]
    public class CommentsController : Controller
    {
        public readonly ICommentsService commentsService;
     

        private readonly UserManager<ATHubUser> _manager;


        public CommentsController(ICommentsService commentsService,
            UserManager<ATHubUser> _manager
            )
        {
            this.commentsService = commentsService;
            this._manager = _manager;
    
        }

        public async Task<IActionResult> Add(string content, int id)
        {
            var currenUser = this._manager.GetUserAsync(HttpContext.User).Result;
            var commentId = await this.commentsService.Add(content, currenUser, id);
            return this.RedirectToAction("Index", "Home", new { area = "" });

        }
    }
}
