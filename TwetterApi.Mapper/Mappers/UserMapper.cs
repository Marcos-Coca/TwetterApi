using System;
using System.Data.SqlClient;
using TwetterApi.Domain.DTOs;
using TwetterApi.Domain.Interfaces.Mappers;
using TwetterApi.Domain.Models.Request;

namespace TwetterApi.Mapper.Mappers
{
     public class UserMapper : IUserMapper
    {
        public UserDTO Map(SqlDataReader reader)
        {
            return new UserDTO
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
        }

        public UserDTO Map(RegisterRequest model)
        {
            return new UserDTO
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Password = model.Password
            };

        }
    }
}
