using ATHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Data
{
    public class DbInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ATHubDbContext>();


                // TODO: move sample data out into JSON files
                if (!context.Users.Any())
                {
                    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                    var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                    await CreateUser(userManager, "admin@gmail.com", "1234567", "Bulgaria");
                    await CreateUser(userManager, "gosho@gmail.com", "1234567", "England");
                   

                    await CreateRole(roleManager, "Administrators");
                    await AddUserToRole(userManager, "admin@gmail.com", "Administrators");

                    context.SaveChanges();
                }
            }
        }
        private static async Task CreateUser(UserManager<User> userManager,
    string email, string password, string country)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
               
            };

            var userCreateResult = await userManager.CreateAsync(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }
        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var roleCreateResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleCreateResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", roleCreateResult.Errors));
            }
        }

        private static async Task AddUserToRole(UserManager<User> userManager, string username, string roleName)
        {
            var user = await userManager.FindByEmailAsync(username);

            var addRoleResult = await userManager.AddToRoleAsync(user, roleName);
            if (!addRoleResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", addRoleResult.Errors));
            }
        }

    }
}
