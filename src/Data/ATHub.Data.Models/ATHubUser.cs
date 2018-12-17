using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            
            this.Videos = new List<Video>();
        }
        public int? UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Video> Videos { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

      
        public int PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
    }
}
