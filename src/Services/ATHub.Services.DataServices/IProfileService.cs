using ATHub.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Services.DataServices
{
    public interface IProfileService
    {
        Task<int> UploadImg(IFormFile file, ATHubUser user);
    }
}
