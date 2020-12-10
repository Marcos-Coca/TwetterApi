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
        public Entities.Type Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TweetUserResponse User { get; set; }
    }

    public class TweetUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
