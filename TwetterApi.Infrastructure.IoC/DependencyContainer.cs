using TwetterApi.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwetterApi.Domain.Repositories;
using TwetterApi.DataAccess.Repositories;
using TwetterApi.Application.UserService;
using TwetterApi.Application.AuthService;
using TwetterApi.Application.TweetService;

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

            // Application
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITweetService, TweetService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
