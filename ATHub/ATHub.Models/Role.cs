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
        }

        public Role(string name)
            : base(name)
        {
           
        }

        public ICollection<UserRole> Users { get; set; }
    }
}
