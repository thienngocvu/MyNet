using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using MyNet.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyNet.Application.Common;
using MyNet.Application.Common.Settings;
using MyNet.Application.Interfaces;
using MyNet.Domain.Entities;
using MyNet.Infrastructure.Persistences;
using MyNet.Infrastructure.Persistences.Repositories;
using MyNet.Infrastructure.Services;
using System.Text;

namespace MyNet.Infrastructure.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddAuthenticationExt(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure JWT Settings
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            // Add ASP.NET Core Identity
            services.AddIdentity<User, Role>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                
                // User settings
                options.User.RequireUniqueEmail = false;
                
                // SignIn settings
                options.SignIn.RequireConfirmedEmail = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Add JWT Bearer Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers["Token-Expired"] = "true";
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // Add Authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthzPolicy.ADMIN_POLICY, policy => policy.RequireRole(AuthzPolicy.ADMIN_ROLE));
                options.AddPolicy(AuthzPolicy.USER_POLICY, policy => policy.RequireRole(AuthzPolicy.USER_ROLE, AuthzPolicy.ADMIN_ROLE));
            });

            // Register JWT services
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            
            // Register Permission - based Authorization services
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return services;
        }

        public static IApplicationBuilder UseAuthenticationExt(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
