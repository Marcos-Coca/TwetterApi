using TwetterApi.Domain.Entities;

namespace TwetterApi.Domain.Repositories
{
   public interface IRefreshTokenRepository
    {
        void SaveRefreshToken(RefreshToken token);
        RefreshToken GetRefreshToken(string token);
        void UpdateRefreshToken(RefreshToken token);
    }
}
