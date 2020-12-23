using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TwetterApi.Domain.DTOs;
using TwetterApi.Domain.Interfaces.Mappers;

namespace TwetterApi.Mapper.Mappers
{
    public class TwetterMapper : ITweetMapper
    {
        public TweetDTO Map(SqlDataReader reader)
        {
            return new TweetDTO
            {
                Id = (int)(reader["id"]),
                Media = (Media)Convert.ToInt32(reader["media"]),
                Visibitity = (Visibility)Convert.ToInt32(reader["visibility"]),
                Type = (Domain.DTOs.Type)Convert.ToInt32(reader["type"]),
                CreatedAt = (DateTime)reader["created_at"],
                UpdatedAt = (DateTime)reader["updated_at"],
                PhotosUrl = new List<string>(),
                UserId = (int)reader["user_id"],
                Name = (string)reader["name"],
                UserName = (string)reader["user_name"],
                Content = Common.IsDBNull(reader["content"]) ? string.Empty : (string)reader["content"],
                PhotoUrl = Common.IsDBNull(reader["photo_url"]) ? string.Empty : (string)reader["photo_url"]
            };
        }
    }
}
