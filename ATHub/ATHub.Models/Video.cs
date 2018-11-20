using ATHub.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Models
{
    public class Video
    {
        //Id, Name, Author, Description, Link, UploadDate, Uploader, Views, Comments, Category, Type

        public int Id { get; set; }

        public string Name { get; set; }

        public string Performer { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public DateTime UploadDate { get; set; }

        //public int UploaderId { get; set; }
        public User Uploader { get; set; }

        public long Views { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public VideoType Type { get; set; }
    }
}
