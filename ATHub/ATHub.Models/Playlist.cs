using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Models
{
    public class Playlist
    {
        public Playlist()
        {
            this.Videos = new List<VideoPlaylist>();
        }
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<VideoPlaylist> Videos { get; set; }
    }
}
