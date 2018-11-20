using ATHub.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ATHub.Models
{
    public class User : IdentityUser
    {
        //TODO Messages

        public Role Role { get; set; }

        public DateTime? Birthdate { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }    
        public ICollection<Video> Videos { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Playlist> Playlists { get; set; }
    }
}
