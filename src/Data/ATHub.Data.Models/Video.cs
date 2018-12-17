using ATHub.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Data.Models
{
    public class Video
    {

        public Video()
        {
            this.Comments = new List<Comment>();
            this.Playlists = new List<VideoPlaylist>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

       

        public DateTime UploadDate { get; set; }

        public string UploaderId { get; set; }
        public virtual ATHubUser Uploader { get; set; }

        public long Views { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual VideoType Type { get; set; }

        public virtual ICollection<VideoPlaylist> Playlists { get; set; }
    }
}
