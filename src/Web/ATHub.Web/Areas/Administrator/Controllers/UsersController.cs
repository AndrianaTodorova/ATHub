using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Web.Areas.Administrator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace ATHub.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class UsersController : Controller
    {
        public readonly IRepository<ATHubUser> user;
        public readonly IRepository<UserRole> roles;

        public UsersController(IRepository<ATHubUser> user,
            IRepository<UserRole> roles)
        {
            this.user = user;
            this.roles = roles;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageUsers()
        {
            var roless = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            var model = this.user.All().Where(p => p.UserName != this.User.Identity.Name).Select(u => new ManageUserViewModel()
            {
                Username = u.UserName,
                Role = "asd",
                Id = u.Id
            }).ToList();

            var users = new AllUsersViewModel()
            {
                Users = model
            };
            return this.View(users);
        }

        [Authorize(Roles = "Admin")]
        
        public IActionResult Delete(string id)
        {
          
            var currentUser = this.user.All().Select(u => u.Id == id).FirstOrDefault();
            return this.View();
        }

        public IActionResult ChangeRole(string id, string role)
        {
            return this.View();
        }
    }
}
