using System;
using System.Data.SqlClient;
using TwetterApi.Domain.DTOs;
using TwetterApi.Domain.Interfaces.Mappers;
using TwetterApi.Domain.Interfaces.Repositories;



namespace TwetterApi.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserMapper _userMapper;
        public UserRepository(IUserMapper userMapper)
        {
            _userMapper = userMapper;
        }
        public void CreateUser(UserDTO user)
        {
            using var connection = Common.GetConnection();
            using var command = SQL_CREATE_USER(user);

            command.Connection = connection;

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public UserDTO GetUser(int id)
        {
            throw new NotImplementedException();
        }
        public UserDTO GetUserByEmail(string email)
        {
            UserDTO user = null;

            using var connection = Common.GetConnection();
            using var command = SQL_GET_USER_BY_EMAIL(email);

            command.Connection = connection;
            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                user = _userMapper.Map(reader);
            }
            connection.Close();
            return user;
        }
        public UserDTO GetUserByUserName(string userName)
        {
            UserDTO user = null;

            using var connection = Common.GetConnection();
            using var command = SQL_GET_USER_BY_USERNAME(userName);

            command.Connection = connection;

            connection.Open();

            using var reader = command.ExecuteReader();

            while(reader.Read())
            {
                user = _userMapper.Map(reader);
            }

            connection.Close();

            return user;
        }

        #region Private methods

        #region SQL commands
        private static SqlCommand SQL_CREATE_USER(UserDTO user)
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
        private static SqlCommand SQL_GET_USER_BY_USERNAME(string userName)
        {
            string query = "SELECT * FROM [user] WHERE user_name = @userName";

            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@userName", userName);

            return command;
        }
        #endregion
        #endregion

    }
}
