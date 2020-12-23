using TwetterApi.Domain.DTOs;

namespace TwetterApi.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        UserDTO GetUser(int id);
        UserDTO GetUserByEmail(string email);
        UserDTO GetUserByUserName(string userName);
        void CreateUser(UserDTO user);
    }
}
