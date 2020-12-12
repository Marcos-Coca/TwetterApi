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
            }

            connection.Close();

            return tweet;
        }

        public List<Tweet> GetTweets()
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

        #endregion

        private static Tweet ReadTweet(SqlDataReader reader) => new Tweet
        {
            Id = (int)(reader["id"]),
            Media = (Media)reader["media"],
            Visibitity = (Visibility)reader["visibility"],
            Type = (Entities.Type)reader["type"],
            CreatedAt = (DateTime)reader["created_at"],
            UpdatedAt = (DateTime)reader["updated_at"],
            UserId = (int)reader["user_id"],
            Name = (string)reader["name"],
            UserName = (string)reader["user_name"],
            Content = Common.IsDBNull(reader["content"]) ? string.Empty : (string)reader["content"],
            PhotoUrl = Common.IsDBNull(reader["photo_url"]) ? string.Empty : (string)reader["photo_url"]
        };

        #endregion

    }
}
