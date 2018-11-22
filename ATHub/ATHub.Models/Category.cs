using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Models
{
    public class Category
    {
        public Category()
        {
            this.Videos = new List<Video>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Video> Videos { get; set; }
    }
}
