using System;
using System.Data.SqlClient;
using TwetterApi.Domain.DTOs;
using TwetterApi.Domain.Interfaces.Mappers;
using TwetterApi.Domain.Interfaces.Repositories;


namespace TwetterApi.DataAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IRefreshTokenMapper _refreshTokenMapper;
        public RefreshTokenRepository(IRefreshTokenMapper refreshTokenMapper)
        {
            _refreshTokenMapper = refreshTokenMapper;
        }
        public RefreshTokenDTO GetRefreshToken(string token)
        {

            RefreshTokenDTO refreshToken = null;

            using var connection = Common.GetConnection();
            using var command = SQL_GET_REFRESHTOKEN(token);

            command.Connection = connection;

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                refreshToken = _refreshTokenMapper.Map(reader);
            }
            connection.Close();

            return refreshToken;
        }
        public void SaveRefreshToken(RefreshTokenDTO token)
        {

            using var connection = Common.GetConnection();
            using var command = SQL_SAVE_REFRESHTOKEN(token);

            command.Connection = connection;

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateRefreshToken(RefreshTokenDTO token)
        {
            using var connection = Common.GetConnection();
            using var command = SQL_UPDATE_REFRESHTOKEN(token);

            command.Connection = connection;

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }


        #region SQL commands
        private static SqlCommand SQL_GET_REFRESHTOKEN(string token)
        {
            string query = "SELECT *,u.id  as user_id FROM refresh_token as r " +
                "JOIN [user] AS u ON u.id = r.user_id " +
                "WHERE r.token = @token";

            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@token", token);

            return command;

        }
        private static SqlCommand SQL_SAVE_REFRESHTOKEN(RefreshTokenDTO token)
        {

            string query = "INSERT INTO refresh_token ([token],[expires]" +
                            ",[created_at],[created_by_ip],[user_id]) " +
                            "VALUES(@token,@expires,@created_at,@created_by_ip,@user_id)";

            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@token", token.Token);
            command.Parameters.AddWithValue("@expires", token.Expires);
            command.Parameters.AddWithValue("@created_at", token.CreatedAt);
            command.Parameters.AddWithValue("@created_by_ip", token.CreatedByIp);
            command.Parameters.AddWithValue("@user_id", token.UserId);

            return command;

        }
        private static SqlCommand SQL_UPDATE_REFRESHTOKEN(RefreshTokenDTO token)
        {
            string query = "UPDATE refresh_token SET revoked=@revoked, " +
                "revoked_by_ip=@revoked_by_ip, replaced_by_token=@replaced_by_token " +
                "WHERE token = @token";


            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@token", token.Token);
            command.Parameters.AddWithValue("@revoked", token.Revoked);
            command.Parameters.AddWithValue("@revoked_by_ip", token.RevokedByIp);
            command.Parameters.AddWithValue("@replaced_by_token", Common.DbNullIfNull(token.ReplacedByToken));


            return command;

        }
        #endregion

    }
}
