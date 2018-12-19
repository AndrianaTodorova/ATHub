using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Services.DataServices
{
    public interface IPlaylistService
    {
        Task<int> AddToPlaylist(int id, ATHubUser currentUser);
        MyPlaylistViewModel GetPlaylistModel(string username);

        Task<int> Remove(int id, ATHubUser currentUser);


    }
}
