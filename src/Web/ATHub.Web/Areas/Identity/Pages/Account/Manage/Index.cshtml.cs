﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ATHub.Data.Common;
using ATHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ATHub.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ATHubUser> _userManager;
        private readonly SignInManager<ATHubUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<UserProfile> userProfile;
        private readonly IRepository<ATHubUser> user;

        public IndexModel(
            UserManager<ATHubUser> userManager,
            SignInManager<ATHubUser> signInManager,
            IEmailSender emailSender,
            IRepository<UserProfile> userProfile,
            IRepository<ATHubUser> user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            this.userProfile = userProfile;
            this.user = user;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Birthdate")]
            public DateTime? Birthdate { get; set; }

            [Display(Name = "Country")]
            public string Country { get; set; }

            [Display(Name = "Facebook Link")]
            public string FacebookLink { get; set; }

            [Display(Name = "Instagram Link")]
            public string InstagramLink { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);


            Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,

            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }
            if (user.UserProfileId != null)
            {
                if (this.Input.Birthdate != null)
                {
                    user.UserProfile.Birthdate = this.Input.Birthdate;
                }
                if (this.Input.Country != null)
                {
                    user.UserProfile.Country = this.Input.Country;
                }
                if (this.Input.FacebookLink != null)
                {
                    user.UserProfile.FacebookLink = this.Input.FacebookLink;
                }
                if (this.Input.PhoneNumber != null)
                {
                    user.UserProfile.Phone = this.Input.PhoneNumber;
                }
                if (this.Input.InstagramLink != null)
                {
                    user.UserProfile.InstagramLink = this.Input.InstagramLink;
                }

                await this.userProfile.SaveChangesAsync();

            }
            else
            {
                var profile = new UserProfile()
                {

                    Phone = this.Input.PhoneNumber,
                    Birthdate = this.Input.Birthdate,
                    Country = this.Input.Country,
                    FacebookLink = this.Input.FacebookLink,
                    InstagramLink = this.Input.InstagramLink, 
                    
                };
                await this.userProfile.AddAsync(profile);
                await this.userProfile.SaveChangesAsync();
                user.UserProfileId = profile.Id;
                user.UserProfile = profile;
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
