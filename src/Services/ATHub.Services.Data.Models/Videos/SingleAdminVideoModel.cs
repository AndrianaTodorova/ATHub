using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Services.Data.Models
{
    public class SingleAdminVideoModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UploadedOn { get; set; }

        public string Author { get; set; }

        public string DeletedOn { get; set; }
    }
}
