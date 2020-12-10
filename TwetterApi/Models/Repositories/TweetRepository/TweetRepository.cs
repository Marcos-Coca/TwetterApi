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
            this._dbContext = dbContext;
        }

        public Tweet GetTweet(int id)
        {
            string query = "SELECT t.*,u.name,u.user_name,u.photo_url " +
                           "FROM tweet as t JOIN [user] as u ON u.id = t.user_id WHERE t.id = @id";

            Tweet tweet = new Tweet();

            using (var connection = _dbContext.Connect())
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tweet = SetTweet(reader);
                }

                connection.Close();
            }

            return tweet;
        }

        public List<Tweet> GetTweets()
        {
            throw new NotImplementedException();
        }

        private static Tweet SetTweet(SqlDataReader reader)
        {
            return new Tweet()
            {
                Id = (int)reader["id"],
                Content = (string)reader["content"],
                Type = (Entities.Type)reader["type"],
                Visibitity = (Visibility)reader["visibility"],
                Media = (Media)reader["media"],
                CreatedAt = (DateTime)reader["created_at"],
                UpdatedAt = (DateTime)reader["updated_at"],
                User = new User()
                {
                    Id = (int)reader["user_id"],
                    Name = (string)reader["name"],
                    UserName = (string)reader["user_name"],
                    PhotoUrl = (string)reader["photo_url"],
                    HeaderUrl = (string)reader["header_url"],
                    Biography = (string)reader["biography"],
                    Password = (string)reader["password"],
                    BirthDate = (DateTime)reader["email"],
                    Email = (string)reader["email"]
                }
            };
        }
    }
}
