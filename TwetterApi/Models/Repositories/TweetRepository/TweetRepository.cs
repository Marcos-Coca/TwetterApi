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
            string query = "SELECT t.*,u.name,u.user_name,u.photo_url " +
                           "FROM tweet as t JOIN [user] as u ON u.id = t.user_id WHERE t.id = @id";

            Tweet tweet = null;

            using (var connection = _dbContext.Connect())
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tweet = DataAdapter.AdaptTweet(reader);
                }

                connection.Close();
            }

            return tweet;
        }

        public List<Tweet> GetTweets()
        {
            throw new NotImplementedException();
        }

    }
}
