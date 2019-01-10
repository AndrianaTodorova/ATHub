using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Services.Data.Models
{
    public class CategoriesViewModel
    {
        public string Name { get; set; }

        public IList<VideoModel> Videos { get; set; }
    }
}
