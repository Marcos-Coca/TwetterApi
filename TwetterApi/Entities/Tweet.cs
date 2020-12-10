using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models;

namespace TwetterApi.Entities
{
    public enum Type
    {
        Post = 1,
        Retweet = 2,
        Comment = 3,
        Like = 4
    }
    public enum Visibility
    {
        Public = 1,
        OnlyFollower = 2
    }
    public enum Media
    {
        No = 0,
        Yes = 1
    }
    public class Tweet
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Visibility Visibitity { get; set; }
        public Type Type { get; set; }
        public Media Media { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User User { get; set; }
        public string? MediaUrl { get; set; }
    }
}
