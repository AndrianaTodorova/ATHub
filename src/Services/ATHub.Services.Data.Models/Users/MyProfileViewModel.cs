using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Services.Data.Models
{
    public class MyProfileViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public DateTime? Birthdate { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public string FacebookLink { get; set; }

        public string InstagramLink { get; set; }
    }
}
