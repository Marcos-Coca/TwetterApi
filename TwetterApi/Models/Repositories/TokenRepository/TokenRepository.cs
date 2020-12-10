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
            string query = "SELECT r.*,u.* FROM refresh_token as r" +
                " JOIN [user] AS u ON u.id = r.user_id" +
                "WHERE r.token = @token";

            RefreshToken refreshToken = null;

            using (var connection = _dbContext.Connect())
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@token", token);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    refreshToken = DataAdapter.AdaptRefreshToken(reader);
                }
                connection.Close();
                
                return refreshToken;
            }
        }

        public void SaveRefreshToken(RefreshToken token)
        {
            string query = "INSERT INTO refresh_token ([token],[expires]" +
                            ",[created_at],[created_by_ip],[user_id])" +
                            "VALUES(@token,@expires,@created_at,@created_by_ip,@user_id)";

            using (var connection = _dbContext.Connect())
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@token", token.Token);
                command.Parameters.AddWithValue("@expires", token.Expires);
                command.Parameters.AddWithValue("@created_at", token.CreatedAt);
                command.Parameters.AddWithValue("@created_by_ip", token.CreatedByIp);
                command.Parameters.AddWithValue("@user_id", token.User.Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


    }
}
