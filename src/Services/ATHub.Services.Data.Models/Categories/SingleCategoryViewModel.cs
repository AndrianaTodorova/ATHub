using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Services.Data.Models
{
    public class SingleCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CreatedOn { get; set; }

        public string DeletedOn { get; set; }

        public int VideosCount { get; set; }
    }
}
