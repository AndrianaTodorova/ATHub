using Google.GData.Client;
using Google.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATHub.Web.Models.Home
{
    public class VideoHelper
    {

        const string YT_CHANNEL_NAME = "blendercookie";
        const string YT_DEVELOPER_ID = "AIzaSyD-tHbj4UD4kzl3_YfAbGtqNGpvmIr3SgQ";

        //public static List<VideoModel> GetVideos()
        //{
        //    YouTubeRequestSettings ytSettings = new YouTubeRequestSettings("YouTubeTest", YT_DEVELOPER_ID);
        //    YouTubeRequest ytRequest = new YouTubeRequest(ytSettings);
        //    string feedURL = String.Format("http://gdata.youtube.com/feeds/api/users/{0}/uploads?orderby=published", YT_CHANNEL_NAME);
        //    Feed<Video> videoFeed = ytRequest.Get<Video>(new Uri(feedURL));
          
        //    return (from video in videoFeed.Entries
        //            select new VideoModel() { YouTubeMovieID = video.VideoId, YouTubeMovieTitle = video.Title }).ToList();
        //}
    }
}
