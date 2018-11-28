using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Data.Models
{
    // Add profile data for application users by adding properties to the ATHubUser class
    public class ATHubUser : IdentityUser
    {
        public ATHubUser()
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
