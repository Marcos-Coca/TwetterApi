using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;
using TwetterApi.Models.DBContext;

namespace TwetterApi.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContext _dbContext;

        public UserRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            string query = "SELECT * FROM [user] AS u WHERE u.email = @email";
            User user = new User();

            using(var connection = _dbContext.Connect())
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);

                connection.Open();
                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    Console.WriteLine(reader);
                    user = SetUser(reader);
                }
                connection.Close();
            }
            return user;
        }
        public void SaveRefreshToken(RefreshToken token)
        {
            string query = "INSERT INTO [dbo].[refresh_token]([token],[expires]" +
                            ",[created_at],[created_by_ip],[revoked],[revoked_by_ip],[replaced_by_token]" +
                            ",[user_id]) VALUES(@token,@expires,@created_at" +
                            ",@created_by_ip,@revoked,@revoked_by_ip,@replaced_by_token,@user_id)";

            using(var connection = _dbContext.Connect())
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@token", token.Token);
                command.Parameters.AddWithValue("@expires", token.Expires);
                command.Parameters.AddWithValue("@created_at", token.CreatedAt);
                command.Parameters.AddWithValue("@created_by_ip", token.CreatedByIp);
                command.Parameters.AddWithValue("@revoked", token.Revoked);
                command.Parameters.AddWithValue("@revoked_by_ip", token.RevokedByIp);
                command.Parameters.AddWithValue("@replaced_by_token", token.ReplacedByToken);
                command.Parameters.AddWithValue("@user_id", token.User.Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static User SetUser(SqlDataReader reader)
        {
            return new User()
            {
                Id = (int?)reader["id"],
                Name = (string?)reader["name"],
                UserName = (string?)reader["user_name"],
                Email = (string?)reader["email"],
                Password = (string?)reader["password"],
                Biography = (string?)reader["biography"],
                BirthDate = (DateTime?)reader["birth_date"],
                PhotoUrl = (string?)reader["photo_url"],
                HeaderUrl = (string?)reader["header_url"]
            };
        }

    }
}
