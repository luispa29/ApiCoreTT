using Application;
using Domain.Interfaces;
using Infrastructure;

namespace ApiCore
{
    public static class Dependency
    {
        public static IServiceCollection AddDependencyDeclaration(this IServiceCollection services)
        {

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            #region Usuario

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            #endregion


            return services;
        }

    }
}
