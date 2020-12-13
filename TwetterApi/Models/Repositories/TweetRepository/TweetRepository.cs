using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models.DBContext;
using TwetterApi.Entities;


namespace TwetterApi.Models.Repositories
{
    public class TweetRepository : ITweetRepository
    {
        private IDbContext _dbContext;
        public TweetRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Tweet GetTweet(int id)
        {
            Tweet tweet = null;

            using var connection = _dbContext.Connect();
            using var command = SQL_GET_TWEET(id);

            command.Connection = connection;
            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tweet = ReadTweet(reader);

                if (tweet.Media == Media.Yes)
                    tweet.PhotosUrl = GetTweetPhotos(tweet.Id);
            }

            connection.Close();

            return tweet;
        }

        public List<Tweet> GetTweets()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Tweet> GetUserAllTweets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tweet> GetUserPublicsTweets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tweet> GetUserPostTweets()
        {
            throw new NotImplementedException();
        }


        #region Private methods

        #region SQL commands
        private static SqlCommand SQL_GET_TWEET(int tweetId)
        {
            string query = "SELECT *,u.id as user_id " +
                           "FROM tweet as t JOIN [user] as u ON u.id = t.user_id WHERE t.id = @id";

            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", tweetId);

            return command;
        }

        private static SqlCommand SQL_GET_TWEET_PHOTOS(int tweetId)
        {
            string query = "SELECT * FROM tweet_photo WHERE tweet_id = @id";

            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", tweetId);

            return command;
        }

        #endregion
        private List<string> GetTweetPhotos(int tweetId)
        {

            List<string> photosUrls = new List<string>();

            using var connection = _dbContext.Connect();
            using var command = SQL_GET_TWEET_PHOTOS(tweetId);

            command.Connection = connection;
            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string url = Convert.ToString(reader["url"]);
                photosUrls.Add(url);
            }
            connection.Close();

            return photosUrls;
        }

        private static Tweet ReadTweet(SqlDataReader reader) => new Tweet
        {
            Id = (int)(reader["id"]),
            Media = (Media)Convert.ToInt32(reader["media"]),
            Visibitity = (Visibility)Convert.ToInt32(reader["visibility"]),
            Type = (Entities.Type)Convert.ToInt32(reader["type"]),
            CreatedAt = (DateTime)reader["created_at"],
            UpdatedAt = (DateTime)reader["updated_at"],
            PhotosUrl = new List<string>(),
            UserId = (int)reader["user_id"],
            Name = (string)reader["name"],
            UserName = (string)reader["user_name"],
            Content = Common.IsDBNull(reader["content"]) ? string.Empty : (string)reader["content"],
            PhotoUrl = Common.IsDBNull(reader["photo_url"]) ? string.Empty : (string)reader["photo_url"],
        };

        public IEnumerable<Tweet> GetUserTweets()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
