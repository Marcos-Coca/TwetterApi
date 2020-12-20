using Microsoft.AspNetCore.Mvc;
using TwetterApi.Application.TweetService;

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

            if (tweet == null) return NotFound();

            return Ok(tweet);
        }

    }
}
