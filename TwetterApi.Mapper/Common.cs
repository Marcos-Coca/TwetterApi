using System;
using System.Collections.Generic;
using System.Text;

namespace TwetterApi.Mapper
{
    class Common
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
    }
}
