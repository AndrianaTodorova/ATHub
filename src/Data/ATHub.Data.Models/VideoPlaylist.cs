using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Data.Models
{
    public class VideoPlaylist
    {
        public int Id { get; set; }

        public int VideoId { get; set; }

        public Video Video { get; set; }

        public int PlaylistId { get; set; }

        public Playlist Playlist { get; set; }
    }
}
