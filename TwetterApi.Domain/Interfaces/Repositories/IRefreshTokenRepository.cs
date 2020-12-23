using TwetterApi.Domain.DTOs;

namespace TwetterApi.Domain.Interfaces.Repositories
{
   public interface IRefreshTokenRepository
    {
        void SaveRefreshToken(RefreshTokenDTO token);
        RefreshTokenDTO GetRefreshToken(string token);
        void UpdateRefreshToken(RefreshTokenDTO token);
    }
}
