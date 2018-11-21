using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ATHub.Data;
using ATHub.Models;
using ATHub.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ATHub.Web.Areas.Identity.Pages.Account
{
    public class EditProfileModel : PageModel
    {
       
        private readonly UserManager<User> _userManager;
        private readonly ATHubDbContext db;

        public EditProfileModel(
            UserManager<User> userManager, ATHubDbContext context
            )
        {
            _userManager = userManager;
            db = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
          
           
            [Display(Name = "PictureLink")]
            public string PictureLink { get; set; }

            
        
            [Display(Name = "Country")]
            public string Country { get; set; }

        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                user.Country = Input.Country;
                if(user.Image == null)
                {
                    var image = new Image()
                    {
                        Url = Input.PictureLink
                    };
                    user.Image = image;
                }
                user.Image.Url = Input.PictureLink;
                await db.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}