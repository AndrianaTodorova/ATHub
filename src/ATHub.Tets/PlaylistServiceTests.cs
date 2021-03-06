﻿using ATHub.Data.Common;
using ATHub.Services.DataServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Tets
{
    public class PlaylistServiceTests : BaseServiceTests
    {
        private IPlaylistService PlaylistService => this.ServiceProvider.GetRequiredService<IPlaylistService>();

        [Test]
        public void AddToPlaylistInvalidId()
        {
            var exception = Assert.ThrowsAsync<NullReferenceException>(() =>
               this.PlaylistService.AddToPlaylist(1, null));
            Assert.That(string.Format(ServicesDataConstants.NullVideo, 1).Equals(exception.Message));

        }

        [Test]
        public void RemoveInvalidPlaylist()
        {
            var exception = Assert.ThrowsAsync<NullReferenceException>(() =>
              this.PlaylistService.Remove(4, null));
            Assert.That((ServicesDataConstants.VideoAlreadyExists).Equals(exception.Message));
        }
    }
}
