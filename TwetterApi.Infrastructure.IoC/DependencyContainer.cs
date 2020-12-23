using TwetterApi.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwetterApi.Mapper.Mappers;
using TwetterApi.DataAccess.Repositories;
using TwetterApi.Application.UserService;
using TwetterApi.Application.AuthService;
using TwetterApi.Application.TweetService;
using TwetterApi.Domain.Interfaces.Mappers;
using TwetterApi.Domain.Interfaces.Repositories;


namespace TwetterApi.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITweetRepository, TweetRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            //Application
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITweetService, TweetService>();

            //Mappers
            services.AddScoped<IUserMapper, UserMapper>();
            services.AddScoped<ITweetMapper, TwetterMapper>();
            services.AddScoped<IRefreshTokenMapper, RefreshTokenMapper>();
        }
    }
}
