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
        private ITweetRepository _tweetRepository;

        public TweetService(ITweetRepository tweetRepository)
        {
            this._tweetRepository = tweetRepository;

        }

        public TweetReponse GetTweet(int id)
        {
            Tweet tweet = _tweetRepository.GetTweet(id);
            return SetTweetResponse(tweet);
        }

        public List<TweetReponse> GetTweets()
        {
            List<Tweet> tweets = _tweetRepository.GetTweets();
            return (List<TweetReponse>)tweets.Select(tweet => SetTweetResponse(tweet));
        }

        private static TweetReponse SetTweetResponse(Tweet tweet)
        {
            return new TweetReponse()
            {
                Id = tweet.Id,
                Content = tweet.Content,
                Type = tweet.Type,
                CreatedAt = tweet.CreatedAt,
                UpdatedAt = tweet.UpdatedAt
            };
        }
    }
}
