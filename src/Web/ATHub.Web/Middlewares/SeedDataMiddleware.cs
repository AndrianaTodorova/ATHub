using ATHub.Data;
using ATHub.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Middlewares
{
    public class SeedDataMiddleware
    {
        private const string ROLE_ADMIN = "Admin";
        private const string ROLE_USER = "User";

        private readonly RequestDelegate _next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ATHubUser> userManager,
                                      RoleManager<Role> roleManager, ATHubContext db)
        {
            SeedRoles(roleManager).GetAwaiter().GetResult();

            SeedUserInRoles(userManager).GetAwaiter().GetResult();

            await _next(context);
        }

        private static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(ROLE_ADMIN))
            {
                await roleManager.CreateAsync(new Role(ROLE_ADMIN));
            }

            if (!await roleManager.RoleExistsAsync(ROLE_USER))
            {
                await roleManager.CreateAsync(new Role(ROLE_USER));
            }
        }

        private static async Task SeedUserInRoles(UserManager<ATHubUser> userManager)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new ATHubUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",

                };

                var password = "123456";

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, ROLE_ADMIN);
                }
            }
        }
    }
}
