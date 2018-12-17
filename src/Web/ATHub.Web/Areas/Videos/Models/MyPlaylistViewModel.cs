using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Videos.Models
{
    public class MyPlaylistViewModel
    {
        public int VideosCount { get; set; }
        public ICollection<PlaylistVideos> Videos { get; set; }
    }
}
