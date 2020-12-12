using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models.Options;

namespace TwetterApi.Models.DBContext
{
    public class TweeterContext : IDbContext
    {
        private readonly DBOptions _options;
        private string connectionString;

        public TweeterContext(IOptions<DBOptions> options)
        {
            _options = options.Value;
            connectionString = $"Data Source={_options.Server};"+
                $"Initial Catalog={_options.Database};"+
                $"User={_options.User};"+
                $"Password={_options.Password}";
        }

        public SqlConnection Connect()
        {
            return new SqlConnection(connectionString);
        }
    }
}
