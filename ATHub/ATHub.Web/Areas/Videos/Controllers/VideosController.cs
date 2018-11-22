using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Videos.Controllers
{
    public class VideosController : Controller
    {
        [Area("Videos")]
        public IActionResult Create()
        {
            var role = this.User.IsInRole("Admin");
            return View();
        }
    }
}
