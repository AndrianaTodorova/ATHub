using ATHub.Data;
using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;

namespace ATHub.Tets
{
    [TestFixture]
    public abstract class BaseServiceTests
    {
        protected ATHubUser uploader;
        protected BaseServiceTests()
        {
            var services = this.SetServices();

            this.ServiceProvider = services.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<ATHubContext>();
        }

        [TearDown]
        public void TearDown()
        {
            this.DbContext.Database.EnsureDeleted();
            this.SetServices();
        }
        protected IServiceProvider ServiceProvider { get; set; }

        protected ATHubContext DbContext
        {
            get;set;
        }

        [SetUp]
        public void AddTestingUserToDb()
        {
            var hasUser = this.DbContext.Users.Any(u => u.UserName == "testName");
            if (!hasUser)
            {
                var result = new ATHubUser
                {
                    
                    UserName = "testName",
                    Email = "test@mail.bg",
                };
                this.DbContext.Users.Add(result);
                this.DbContext.SaveChangesAsync();
                this.uploader = result;
            }
            else
            {
                this.uploader = this.DbContext.Users.Where(x => x.UserName == "testName").FirstOrDefault();
            }

        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ATHubContext>(
                opt => opt.UseInMemoryDatabase("DbBaza").EnableSensitiveDataLogging());

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

        //public void Dispose()
        //{
        //    DbContext.Database.EnsureDeleted();
        //    this.SetServices();
        //}
    }
}
