using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string AuthorId { get; set; }
        public virtual ATHubUser Author { get; set; }

        public DateTime WrittenDate { get; set; }


    }
}
