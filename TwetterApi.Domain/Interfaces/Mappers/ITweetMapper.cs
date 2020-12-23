using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using TwetterApi.Domain.DTOs;

namespace TwetterApi.Domain.Interfaces.Mappers
{
    public interface ITweetMapper
    {
        TweetDTO Map(SqlDataReader reader);
    }
}
