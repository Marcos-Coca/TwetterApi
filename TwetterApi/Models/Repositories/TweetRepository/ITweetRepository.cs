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
        List<Tweet> GetTweets();

    }
}
