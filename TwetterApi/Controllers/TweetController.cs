using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models.Response;
using TwetterApi.Services;

namespace TwetterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private ITweetService _tweetService;

        public TweetController(ITweetService tweetService)
        {
            this._tweetService = tweetService;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tweets = _tweetService.GetTweets();

            return Ok(tweets);
        }

        [HttpGet("{tweetId}")]
        public IActionResult Get(int tweetId)
        {
            var tweet = _tweetService.GetTweet(tweetId);

            return Ok(tweet);
        }

    }
}
