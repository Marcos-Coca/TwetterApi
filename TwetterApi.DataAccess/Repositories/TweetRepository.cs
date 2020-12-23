using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TwetterApi.Domain.DTOs;
using TwetterApi.Domain.Interfaces.Mappers;
using TwetterApi.Domain.Interfaces.Repositories;



namespace TwetterApi.DataAccess.Repositories
{
    public class TweetRepository : ITweetRepository
    {
        private readonly ITweetMapper _tweetMapper;
        public TweetRepository(ITweetMapper tweetMapper)
        {
            _tweetMapper = tweetMapper;
        }
            

        public TweetDTO GetTweet(int id)
        {
            TweetDTO tweet = null;

            using var connection = Common.GetConnection();
            using var command = SQL_GET_TWEET(id);

            command.Connection = connection;
            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tweet = _tweetMapper.Map(reader);

                if (tweet.Media == Media.Yes)
                    tweet.PhotosUrl = GetTweetPhotos(tweet.Id);
            }

            connection.Close();

            return tweet;
        }

        public List<TweetDTO> GetTweets()
        {
            throw new NotImplementedException();
        }
        public List<TweetDTO> GetUserAllTweets()
        {
            throw new NotImplementedException();
        }

        public List<TweetDTO> GetUserPublicsTweets()
        {
            throw new NotImplementedException();
        }

        public List<TweetDTO> GetUserPostTweets()
        {
            throw new NotImplementedException();
        }
        public List<TweetDTO> GetUserTweets()
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

            using var connection = Common.GetConnection();
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
        #endregion
    }
}
