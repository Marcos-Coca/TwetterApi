using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Domain.Options;

namespace TwetterApi.DataAccess
{
    public static class Common
    {
        
        internal static bool IsDBNull(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return true;
            }
            return false;

        }

        internal static object DbNullIfNull(object value)
        {
            return value ?? DBNull.Value;
        }
        private static string ConnnectionString
        {
            get
            {
                var dbOptionsSection = ConfigurationManager.ConnectionStrings[DbOptions.Db];
                return dbOptionsSection.ConnectionString;
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnnectionString);

        }
    }
}
