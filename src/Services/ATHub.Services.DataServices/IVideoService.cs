using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATHub.Services.DataServices
{
    public interface IVideoService
    {
        Task<int> Create(string name, string description, string link, string category, ATHubUser uploader);
        IEnumerable<VideoModel> GetRandomVideos(int count);

        IEnumerable<VideoModel> SearchVideos(string search);

        DetailsVideoModel GetDetailsVideoModel(int id);

        IEnumerable<CommentsDetailsVideoModel> GetComments(int id);
    }
}
