﻿using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Services.DataServices
{
    public interface IProfileService
    {
        MyProfileViewModel GetProfile(string id);
        Task<int> UploadImg(IFormFile file, ATHubUser user);
    }
}
