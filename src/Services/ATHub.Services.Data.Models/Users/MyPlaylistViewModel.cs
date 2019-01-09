using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Services.Data.Models
{
    public class MyPlaylistViewModel
    {
        public int VideosCount { get; set; }

        public IList<VideoModel> Videos { get; set; }
    }
}
