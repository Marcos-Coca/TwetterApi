using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models.Response;

namespace TwetterApi.Services
{
       public interface ITweetService
    {
        TweetReponse  GetTweet(int id);
        List<TweetReponse> GetTweets();

    }
}
