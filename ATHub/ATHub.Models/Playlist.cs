﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Models
{
    public class Playlist
    {
        //Id, Videos, User, CreatedOn

        public int Id { get; set; }

        public User User { get; set; }

        public DateTime CreatedOn { get; set; }


        public ICollection<VideoPlaylist> Videos { get; set; }
    }
}
