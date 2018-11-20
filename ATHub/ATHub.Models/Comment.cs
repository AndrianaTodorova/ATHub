using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Models
{
    public class Comment
    {
        //Id, Text, WrritenBy, WrittenDate

        public int Id { get; set; }

        public string Text { get; set; }
        public User WrritenBy { get; set; }

        public DateTime WrittenDate { get; set; }


    }
}
