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
        private IDbContext _dbContext;

        public UserRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateUser(User user)
        {
            string query = "INSERT INTO [user] ([name],[user_name],[email],[birth_date],[password])" +
                " Values(@name,@user_name,@email,@birth_date,@password)";

            using var connection = _dbContext.Connect();

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@user_name", user.UserName);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@birth_date", user.BirthDate);
            command.Parameters.AddWithValue("@password", user.Password);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            string query = "SELECT * FROM [user] WHERE [user].email = @email";
            User user = null;

            using var connection = _dbContext.Connect(); 

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);

            connection.Open();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                user = DataAdapter.AdaptUser(reader);
            }
            connection.Close();
            return user;
        }
       
    }
}
