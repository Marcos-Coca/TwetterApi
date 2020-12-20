using System.Collections.Generic;
using TwetterApi.Domain.Models.Response;

namespace TwetterApi.Application.TweetService
{
       public interface ITweetService
    {
        TweetReponse  GetTweet(int id);
        List<TweetReponse> GetTweets();

    }
}
