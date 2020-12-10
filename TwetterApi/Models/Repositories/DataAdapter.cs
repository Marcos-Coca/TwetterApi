using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;

namespace TwetterApi.Models.Repositories
{
    public class DataAdapter
    {

        public static Tweet AdaptTweet(SqlDataReader reader)
        {
            return new Tweet()
            {
                Id = (int)reader["id"],
                Type = (Entities.Type)reader["type"],
                Visibitity = (Visibility)reader["visibility"],
                Media = (Media)reader["media"],
                CreatedAt = (DateTime)reader["created_at"],
                UpdatedAt = (DateTime)reader["updated_at"],
                Content = (reader["content"] ==null) ? string.Empty : reader["content"].ToString(),
                User = AdaptUser(reader)
            };
        }

        public static User AdaptUser(SqlDataReader reader)
        {
            return new User()
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                UserName = (string)reader["user_name"],
                Email = (string)reader["email"],
                Password = (string)reader["password"],
                BirthDate = (DateTime)reader["birth_date"],
                PhotoUrl = (reader["photo_url"] == null) ? string.Empty : reader["photo_url"].ToString(),
                HeaderUrl = (reader["header_url"] == null) ? string.Empty : reader["header_url"].ToString(),
                Biography = (reader["biography"] == null) ? string.Empty : reader["biography"].ToString()
            };
        }

        public static RefreshToken AdaptRefreshToken(SqlDataReader reader)
        {
            return new RefreshToken()
            {
                Id = (int)reader["id"],
                Token = (string)reader["token"],
                Expires = (DateTime)reader["expires"],
                CreatedAt = (DateTime)reader["created_at"],
                CreatedByIp = (string)reader["created_by_ip"],
                Revoked = (reader["revoked"] == null) ? null : (DateTime)reader["revoked"],
                RevokedByIp = (reader["revoked_by_ip"] == null) ? string.Empty : reader["revoked_by_ip"].ToString(),
                ReplacedByToken = (reader["replaced_by_token"] == null) ? string.Empty : reader["replaced_by_token"].ToString(),
                User = AdaptUser(reader)
            };
        }
        
    }
}
