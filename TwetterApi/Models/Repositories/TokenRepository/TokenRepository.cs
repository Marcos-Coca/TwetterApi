using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;
using TwetterApi.Models.DBContext;

namespace TwetterApi.Models.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private IDbContext _dbContext;
        public TokenRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public RefreshToken GetRefreshToken(string token)
        {
     
            RefreshToken refreshToken = null;

            using var connection = _dbContext.Connect();
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
        public IEnumerable<RefreshToken> GetUserRefreshTokens(int userId)
        {
            var refreshTokens = new List<RefreshToken>();

            using var connection = _dbContext.Connect();
            using var command = SQL_GET_USER_REFRESHTOKENS(userId);

            command.Connection = connection;

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var refreshToken = ReadRefreshToken(reader);
                refreshTokens.Add(refreshToken);
            }
            connection.Close();

            return refreshTokens;
        }
        public void SaveRefreshToken(RefreshToken token)
        {

            using var connection = _dbContext.Connect();
            using var command = SQL_SAVE_REFRESHTOKEN(token);

            command.Connection = connection;
         
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateRefreshToken(RefreshToken token)
        {
            using var connection = _dbContext.Connect();
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
        private static SqlCommand SQL_GET_USER_REFRESHTOKENS(int userId)
        {
            string query = "SELECT * FROM refresh_token as r " +
                "WHERE r.user_id = @id";

            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", userId);

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
            command.Parameters.AddWithValue("@replaced_by_token", token.ReplacedByToken);
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
