
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;

namespace TwetterApi.Models.Repositories
{
    public interface ITokenRepository
    {
        void SaveRefreshToken(RefreshToken token);
        RefreshToken GetRefreshToken(string token);
    }
}
