using System.Collections.Generic;
using TwetterApi.Domain.DTOs;

namespace TwetterApi.Domain.Interfaces.Repositories
{
    public interface ITweetRepository
    {
        TweetDTO GetTweet(int id);
        List<TweetDTO> GetUserTweets();
        List<TweetDTO> GetUserPublicsTweets();
        List<TweetDTO> GetUserPostTweets();
        List<TweetDTO> GetTweets();
    }
}
