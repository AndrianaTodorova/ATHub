using ATHub.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Videos = new List<Video>();
            this.CreatedOn = DateTime.UtcNow;
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
    }
}
