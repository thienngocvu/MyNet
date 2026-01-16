using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Application.Interfaces;
using MyNet.Application.Interfaces.Persistence;
using MyNet.Infrastructure.Persistences;
using MyNet.Infrastructure.Persistences.Repositories;
using MyNet.Application.Services;
using MyNet.Infrastructure.Services;

namespace MyNet.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServiceExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>((service, option) =>
            {
                var connectionString = configuration["ConnectionStrings:Default"];
                option.UseNpgsql(connectionString).EnableSensitiveDataLogging();
            });

            #region"repositories"
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoginLogRepository, LoginLogRepository>();
            services.AddScoped<IFunctionRepository, FunctionRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            #endregion


            #region"services"
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            #endregion

            return services;
        }
    }
}
