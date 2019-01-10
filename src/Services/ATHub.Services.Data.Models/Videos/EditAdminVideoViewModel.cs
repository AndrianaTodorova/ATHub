using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Services.Data.Models
{
    public class EditAdminVideoViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string Link { get; set; }
        
        public HashSet<string> categoryNames { get; set; }

        public string Category { get; set; }
    }
}
