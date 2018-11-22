using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Models
{
    public class Role : IdentityRole
    {
        public Role()
            : this(null)
        {
            this.Users = new List<UserRole>();
        }

        public Role(string name)
            : base(name)
        {
           
        }

        public ICollection<UserRole> Users { get; set; }
    }
}
