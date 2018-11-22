using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public DateTime? Birthdate { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public int? ImageId { get; set; }
        public Image Image { get; set; }
    }
}
