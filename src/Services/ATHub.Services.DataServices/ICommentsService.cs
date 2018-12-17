using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Services.DataServices
{
    public interface ICommentsService
    {
        Task<int> Add(string content, ATHubUser uploader, int id);
    }
}
