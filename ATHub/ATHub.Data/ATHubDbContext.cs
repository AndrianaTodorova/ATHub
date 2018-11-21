using ATHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ATHub.Data
{
    public class ATHubDbContext : IdentityDbContext<User>
    { 
        public ATHubDbContext(DbContextOptions<ATHubDbContext> options)
          : base(options)
        {
        }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Playlist> PlayLists { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }    }
}
