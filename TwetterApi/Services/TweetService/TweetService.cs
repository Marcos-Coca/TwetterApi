using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models;
using TwetterApi.Models.Repositories;
using TwetterApi.Models.Response;
using TwetterApi.Entities;

namespace TwetterApi.Services
{
    public class TweetService : ITweetService
    {
        private readonly ITweetRepository _tweetRepository;

        public TweetService(ITweetRepository tweetRepository)
        {
            _tweetRepository = tweetRepository;

        }

        public TweetReponse GetTweet(int id)
        {
            Tweet tweet = _tweetRepository.GetTweet(id);
            if (tweet == null) return null;
            return new TweetReponse(tweet);
        }

        public List<TweetReponse> GetTweets()
        {
            List<Tweet> tweets = _tweetRepository.GetTweets();
           return (List<TweetReponse>)tweets.Select(tweet => new TweetReponse(tweet));
        }
    }
}
