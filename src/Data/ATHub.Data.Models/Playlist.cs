using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ATHub.Data.Models
{
    public class Playlist
    {
        public Playlist()
        {
            this.Videos = new List<VideoPlaylist>();
        }
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual ATHubUser User { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<VideoPlaylist> Videos { get; set; }
    }
}
