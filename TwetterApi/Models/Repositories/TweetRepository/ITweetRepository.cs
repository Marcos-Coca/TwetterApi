using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;

namespace TwetterApi.Models.Repositories
{
    public interface ITweetRepository
    {
        Tweet GetTweet(int id);
        IEnumerable<Tweet> GetUserTweets();
        IEnumerable<Tweet> GetUserPublicsTweets();
        IEnumerable<Tweet> GetUserPostTweets();
        List<Tweet> GetTweets();
    }
}
