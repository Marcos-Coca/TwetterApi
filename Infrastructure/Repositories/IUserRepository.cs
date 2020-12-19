
using TwetterApi.Domain.Entities;

namespace TwetterApi.Domain.Repositories
{
    public interface IUserRepository
    {
        User GetUser(int id);
        User GetUserByEmail(string email);
        User GetUserByUserName(string userName);
        void CreateUser(User user);
    }
}
