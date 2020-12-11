
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;

namespace TwetterApi.Models.Repositories
{
    public interface IUserRepository
    {
        User GetUser(int id);
        User GetUserByEmail(string email);
        void CreateUser(User user);
    }
}
