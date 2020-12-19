using System.Collections.Generic;
using TwetterApi.Domain.Entities;

namespace TwetterApi.Domain.Repositories
{
    public interface ITweetRepository
    {
        Tweet GetTweet(int id);
        List<Tweet> GetUserTweets();
        List<Tweet> GetUserPublicsTweets();
        List<Tweet> GetUserPostTweets();
        List<Tweet> GetTweets();
    }
}
