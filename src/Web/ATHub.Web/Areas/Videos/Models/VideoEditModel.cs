using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Videos.Models
{
    public class VideoEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string Category { get; set; }

    }
}
