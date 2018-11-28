using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Videos.Models
{
    public class VideoCreateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public DateTime UploadDate { get; set; }

        public string Uploader { get; set; }

        public string Category { get; set; }

    }
}
