﻿using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ATHub.Services.DataServices
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IRepository<Playlist> playlistRepository;

        private readonly IRepository<Video> videoRepository;

        private readonly IRepository<ATHubUser> userRepository;

        IRepository<VideoPlaylist> videoPlaylists;

        public PlaylistService(IRepository<Playlist> playlistRepository,
            IRepository<Video> videoRepository,
            IRepository<VideoPlaylist> videoPlaylists,
            IRepository<ATHubUser> userRepository)
        {
            this.playlistRepository = playlistRepository;
            this.videoRepository = videoRepository;
            this.userRepository = userRepository;
            this.videoPlaylists = videoPlaylists;
        }

        public async Task<int> AddToPlaylist(int id, ATHubUser currentUser)
        {
            var currentVideo = this.videoRepository.All().FirstOrDefault(v => v.Id == id);
            if(currentVideo == null)
            {
                throw new NullReferenceException(string.Format(ServicesDataConstants.NullVideo, id));
            }
            var videoPlaylist = new VideoPlaylist()
            {
                Video = currentVideo,
                VideoId = currentVideo.Id,
            };
            if (currentUser.PlaylistId == 0)
            {
                var playlist = new Playlist()
                {
                    CreatedOn = DateTime.UtcNow,
                    User = currentUser,
                    UserId = currentUser.Id,
                };
                await this.playlistRepository.AddAsync(playlist);
                await this.playlistRepository.SaveChangesAsync();
                currentUser.Playlist = playlist;
                currentUser.PlaylistId = playlist.Id;
                videoPlaylist.PlaylistId = playlist.Id;
                videoPlaylist.Playlist = playlist;
                await this.videoPlaylists.AddAsync(videoPlaylist);
                await this.videoPlaylists.SaveChangesAsync();


            }
            if (currentUser.Playlist.Videos.Any(p => p.Id == id))
            {
                throw new NullReferenceException(ServicesDataConstants.VideoAlreadyExists);
            }
            currentUser.Playlist.Videos.Add(videoPlaylist);
            await this.userRepository.SaveChangesAsync();
            return videoPlaylist.Id;

        }

        public MyPlaylistViewModel GetPlaylistModel(string username)
        {
            var model = this.userRepository.All().Where(u => u.UserName == username).Select(x => x.Playlist).Select(p => new MyPlaylistViewModel()
            {
                VideosCount = p.Videos.Count,
                Videos = p.Videos.OrderBy(v => v.Video.Id).Select(v => new VideoModel()
                {
                    Title = v.Video.Name,
                    Link = this.GetEmbed(v.Video.Link),
                    Id = v.VideoId

                }).ToList()
            }).FirstOrDefault();
         
            return model;
        }

        public async Task<int> Remove(int id, ATHubUser currentUser)
        {
            var currentVideo = this.videoPlaylists.All().FirstOrDefault(v => v.VideoId == id);
            if(currentVideo == null)
            {
                throw new NullReferenceException(ServicesDataConstants.VideoAlreadyExists);
            }
            currentUser.Playlist.Videos.Remove(currentVideo);
            await this.userRepository.SaveChangesAsync();
            return currentVideo.Id;
        }

        private string GetEmbed(string link)
        {

            var uri = new Uri(link);

            var query = HttpUtility.ParseQueryString(uri.Query);

            var videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }
            return videoId;
        }

    }
}
