using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace TwetterApi.Models.DBContext
{
    public interface IDbContext
    {
        SqlConnection Connect();

    }
}
