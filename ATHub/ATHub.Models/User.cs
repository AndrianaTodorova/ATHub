using ATHub.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ATHub.Models
{
    public class User : IdentityUser
    {
        //TODO Messages
        public User()
        {
            this.Roles = new List<UserRole>();
            this.Comments = new List<Comment>();
            this.Playlists = new List<Playlist>();
            this.Videos = new List<Video>();
        }
        public int? UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }
        public ICollection<Video> Videos { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Playlist> Playlists { get; set; }

        public ICollection<UserRole> Roles { get; set; }

    }
}
