using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwetterApi.Models.Repositories
{
    public class Common
    {
        
        public static bool IsDBNull(object value)
        {
            if (value == System.DBNull.Value || value == null)
            {
                return true;
            }
            return false;

        }
    }
}
