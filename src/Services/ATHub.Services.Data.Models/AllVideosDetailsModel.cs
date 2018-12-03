using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Services.Data.Models
{
    public class AllVideosDetailsModel
    {
        public ICollection<DetailsVideoModel> DetailsVideos { get; set; }
    }
}
