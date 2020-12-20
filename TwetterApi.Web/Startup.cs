using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Configuration;
using System.Text;
using TwetterApi.Domain.Options;
using TwetterApi.Infrastructure.IoC;

namespace TwetterApi.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
       
        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TwetterApi", Version = "v1" });
            });

            //Secrets
            var dbOptionsSection = Configuration.GetSection(DbOptions.Db);
            var tokenOptionsSection = Configuration.GetSection(TokenOptions.Token);

            services.Configure<DbOptions>(dbOptionsSection);
            services.Configure<TokenOptions>(tokenOptionsSection);

            //Set Connection String
            SetConnectionString(dbOptionsSection);
            
            //Dependency Injection
            RegisterServices(services);


            //Configure jwt authentication
            var tokenOptions = tokenOptionsSection.Get<TokenOptions>();
            var key = Encoding.ASCII.GetBytes(tokenOptions.JwtTokenSecret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
        private static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }

        private static void SetConnectionString(IConfigurationSection dbSection)
        {
            string connectionString = dbSection.Get<DbOptions>().ConnectionString;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var csSection = config.ConnectionStrings;

            csSection.ConnectionStrings.Add(new ConnectionStringSettings(DbOptions.Db, connectionString));
            config.Save(ConfigurationSaveMode.Modified);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwetterApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
