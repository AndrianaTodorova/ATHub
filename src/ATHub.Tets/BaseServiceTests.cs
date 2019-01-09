using ATHub.Data;
using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ATHub.Tets
{
    public abstract class BaseServiceTests
    {
        protected BaseServiceTests()
        {
            var services = this.SetServices();

            this.ServiceProvider = services.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<ATHubContext>();
        }

        protected IServiceProvider ServiceProvider { get; set; }

        protected ATHubContext DbContext { get; set; }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ATHubContext>(
                opt => opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services
                    .AddIdentity<ATHubUser, Role>(options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredLength = 6;
                    })
                    .AddEntityFrameworkStores<ATHubContext>()
                   // .AddUserStore<ATHubContextFactory>()
                    .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IProfileService, ProfileService>();

            var context = new DefaultHttpContext();
            services.AddSingleton<IHttpContextAccessor>(new HttpContextAccessor { HttpContext = context });

            return services;
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            this.SetServices();
        }
    }
}
