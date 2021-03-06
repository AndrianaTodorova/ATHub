﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ATHub.Web.Models;
using ATHub.Data;
using ATHub.Data.Models;
using ATHub.Data.Common;
using ATHub.Services.DataServices;
using ATHub.Web.Middlewares;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace ATHub.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(opt =>
            { opt.LoginPath = "/Identity/Account/Login"; }
            );

            services.AddDbContext<ATHubContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());
            services.AddIdentity<ATHubUser, Role>(
                 options =>
                 {
                     options.Password.RequiredLength = 6;
                     options.Password.RequireLowercase = false;
                     options.Password.RequireNonAlphanumeric = false;
                     options.Password.RequireUppercase = false;
                     options.Password.RequireDigit = false;
                 })
                .AddEntityFrameworkStores<ATHubContext>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc(
            //    options =>
            //options.Filters
            //.Add(new CustomExceptionFilter())
            )
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IProfileService, ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
            
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // For the wwwroot folder
            string pth = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            Console.WriteLine(pth);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(pth),
                RequestPath = "/StaticFiles"
            });
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSeedDataMiddlewareExtensions();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute", template: "{area:exists}/{controller=Home}/{action=Index}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
