using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;
namespace TwetterApi.Models.Response
{
    public class TweetReponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public Media Media { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> PhotosUrl { get; set; }
        public TweetUserResponse User { get; set; }

        public TweetReponse(Tweet tweet)
        {
            Id = tweet.Id;
            Content = tweet.Content;
            Type = tweet.Type.ToString();
            Media = tweet.Media;
            CreatedAt = tweet.CreatedAt;
            UpdatedAt = tweet.UpdatedAt;
            PhotosUrl = tweet.PhotosUrl;
            User = new TweetUserResponse(tweet);
        }
    }

    public class TweetUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }

        public TweetUserResponse(Tweet tweet)
        {
            Id = tweet.UserId;
            Name = tweet.Name;
            UserName = tweet.UserName;
            PhotoUrl = tweet.PhotoUrl;
        }
    }
}
