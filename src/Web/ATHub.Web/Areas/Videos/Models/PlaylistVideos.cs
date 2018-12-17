using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Videos.Models
{
    public class PlaylistVideos
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Views { get; set; }
    }
}
