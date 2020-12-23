using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using TwetterApi.Domain.DTOs;
using TwetterApi.Domain.Models.Request;

namespace TwetterApi.Domain.Interfaces.Mappers
{
    public interface IUserMapper
    {
        UserDTO Map(SqlDataReader reader);
        UserDTO Map(RegisterRequest model);
    }
}
