using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Services.Data.Models
{
    public class DetailsVideoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public long Views { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string UploadDate { get; set; }

        public string UploaderName { get; set; }

        public IEnumerable<CommentsDetailsVideoModel> Comments { get; set; }
    }
}
