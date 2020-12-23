using System.Linq;
using System.Collections.Generic;
using TwetterApi.Domain.DTOs;
using TwetterApi.Domain.Models.Response;
using TwetterApi.Domain.Interfaces.Repositories;



namespace TwetterApi.Application.TweetService
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
            TweetDTO tweet = _tweetRepository.GetTweet(id);
            if (tweet == null) return null;
            return new TweetReponse(tweet);
        }

        public List<TweetReponse> GetTweets()
        {
            List<TweetDTO> tweets = _tweetRepository.GetTweets();
           return (List<TweetReponse>)tweets.Select(tweet => new TweetReponse(tweet));
        }
    }
}
