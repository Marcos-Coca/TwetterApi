using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TwetterApi.Domain.DTOs;
using TwetterApi.Domain.Interfaces.Mappers;

namespace TwetterApi.Mapper.Mappers
{
    public class RefreshTokenMapper: IRefreshTokenMapper
    {
        public RefreshTokenDTO Map(SqlDataReader reader) => new RefreshTokenDTO
        {
            Id = Convert.ToInt32(reader["id"]),
            Token = Convert.ToString(reader["token"]),
            CreatedAt = Convert.ToDateTime(reader["created_at"]),
            CreatedByIp = Convert.ToString(reader["created_by_ip"]),
            Expires = Convert.ToDateTime(reader["expires"]),
            UserId = Convert.ToInt32(reader["user_id"]),
            Revoked = Common.IsDBNull(reader["revoked"]) ? null : Convert.ToDateTime(reader["revoked"]),
            RevokedByIp = Common.IsDBNull(reader["revoked_by_ip"]) ? null : Convert.ToString(reader["revoked_by_ip"]),
            ReplacedByToken = Common.IsDBNull(reader["replaced_by_token"]) ? null : Convert.ToString(reader["replaced_by_token"]),
        };
    }
}
