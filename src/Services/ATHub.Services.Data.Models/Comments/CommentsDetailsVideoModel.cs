﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Services.Data.Models
{
    public class CommentsDetailsVideoModel
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public string Date { get; set; }

        public string UploaderName { get; set; }
    }
}
