using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Web.Areas.Administrator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class UsersController : Controller
    {
        public readonly IRepository<ATHubUser> user;

        public UsersController(IRepository<ATHubUser> user)
        {
            this.user = user;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageUsers()
        {
            var model = this.user.All().Where(p => p.UserName != this.User.Identity.Name).Select(u => new ManageUserViewModel()
            {
                Username = u.UserName,
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
