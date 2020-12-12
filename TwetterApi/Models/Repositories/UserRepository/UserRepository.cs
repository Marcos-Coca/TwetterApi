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
            using var connection = _dbContext.Connect();
            using var command = SQL_CREATE_USER(user);

            command.Connection = connection;

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
            User user = null;

            using var connection = _dbContext.Connect();
            using var command = SQL_GET_USER_BY_EMAIL(email);

            command.Connection = connection;
            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                user = ReadUser(reader);
            }
            connection.Close();
            return user;
        }

        #region Private methods

        #region SQL commands
        private static SqlCommand SQL_CREATE_USER(User user)
        {
            string query = "INSERT INTO [user] ([name],[user_name],[email],[birth_date],[password])" +
                " Values(@name,@user_name,@email,@birth_date,@password)";

            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@user_name", user.UserName);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@birth_date", user.BirthDate);
            command.Parameters.AddWithValue("@password", user.Password);

            return command;
        }
        private static SqlCommand SQL_GET_USER_BY_EMAIL(string email)
        {
            string query = "SELECT * FROM [user] WHERE email = @email";

            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@email", email);

            return command;
        }
        #endregion
        private static User ReadUser(SqlDataReader reader) => new User
        {
            Id = Convert.ToInt32(reader["id"]),
            Name = Convert.ToString(reader["name"]),
            UserName = Convert.ToString(reader["user_name"]),
            Email = Convert.ToString(reader["email"]),
            Password = Convert.ToString(reader["password"]),
            BirthDate = Convert.ToDateTime(reader["birth_date"]),
            PhotoUrl = Common.IsDBNull(reader["photo_url"]) ? string.Empty : Convert.ToString(reader["photo_url"]),
            Biography = Common.IsDBNull(reader["biography"]) ? string.Empty : Convert.ToString(reader["biography"]),
            HeaderUrl = Common.IsDBNull(reader["header_url"]) ? string.Empty : Convert.ToString(reader["header_url"]),
        };
        #endregion

    }
}
