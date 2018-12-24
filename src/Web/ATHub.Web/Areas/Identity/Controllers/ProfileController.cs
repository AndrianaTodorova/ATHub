﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using ATHub.Data.Models;
using System.Drawing.Imaging;
using Image = ATHub.Data.Models.Image;
using ATHub.Services.DataServices;
using Microsoft.AspNetCore.Identity;

namespace ATHub.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class ProfileController : Controller
    {
        public readonly IProfileService profileService;
        private readonly UserManager<ATHubUser> _manager;
        public ProfileController(IProfileService profileService,
            UserManager<ATHubUser> manager)
        {
            this.profileService = profileService;
            this._manager = manager;
        }
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            var currenUser = this._manager.GetUserAsync(HttpContext.User).Result;
            if(file != null)
            {
                await profileService.UploadImg(file, currenUser);
            }
           
            // after successfully uploading redirect the user
            return RedirectToAction("Index", "Home", new { area = ""});
        }

        public IActionResult MyProfile()
        {
            return this.View();
        }
    }
}