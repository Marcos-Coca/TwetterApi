using System;
using System.Data.SqlClient;
using TwetterApi.DataAccess;
using TwetterApi.Domain.Entities;
using TwetterApi.Domain.Repositories;

namespace TwetterApi.DataAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        public RefreshToken GetRefreshToken(string token)
        {

            RefreshToken refreshToken = null;

            using var connection = Common.GetConnection();
            using var command = SQL_GET_REFRESHTOKEN(token);

            command.Connection = connection;

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                refreshToken = ReadRefreshToken(reader);
            }
            connection.Close();

            return refreshToken;
        }
        public void SaveRefreshToken(RefreshToken token)
        {

            using var connection = Common.GetConnection();
            using var command = SQL_SAVE_REFRESHTOKEN(token);

            command.Connection = connection;

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateRefreshToken(RefreshToken token)
        {
            using var connection = Common.GetConnection();
            using var command = SQL_UPDATE_REFRESHTOKEN(token);

            command.Connection = connection;

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        #region Private methods

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
        private static SqlCommand SQL_SAVE_REFRESHTOKEN(RefreshToken token)
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
        private static SqlCommand SQL_UPDATE_REFRESHTOKEN(RefreshToken token)
        {
            string query = "UPDATE refresh_token SET revoked=@revoked, " +
                "revoked_by_ip=@revoked_by_ip, replaced_by_token=@replaced_by_token " +
                "WHERE token = @token";


            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@revoked", token.Revoked);
            command.Parameters.AddWithValue("@revoked_by_ip", token.RevokedByIp);
            command.Parameters.AddWithValue("@replaced_by_token", "").Value = token.ReplacedByToken ?? (object)DBNull.Value;
            command.Parameters.AddWithValue("@token", token.Token);

            return command;

        }
        #endregion
        private static RefreshToken ReadRefreshToken(SqlDataReader reader) => new RefreshToken
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
        #endregion
    }
}
