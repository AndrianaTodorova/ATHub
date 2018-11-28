using ATHub.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ATHub.Data
{
    public class ATHubContext : IdentityDbContext<ATHubUser, Role, string>
    {
        public ATHubContext(DbContextOptions<ATHubContext> options)
            : base(options)
        {
        }
        public DbSet<Video> Videos { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Playlist> PlayLists { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<UserProfile> UsersProfiles { get; set; }

        public DbSet<UserRole> UsersRoles { get; set; }

        public DbSet<VideoPlaylist> VideosPlaylists { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
