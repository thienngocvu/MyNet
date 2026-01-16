using MyNet.API.Filters;
using MyNet.Application.Common.Constants;

namespace MyNet.API.Extensions
{
    public static class HttpContextExtension
    {
        public static IServiceCollection AddCustomControllerExt(this IServiceCollection services)
        {
            services.AddScoped<GlobalExceptionFilter>();
            services.AddControllers(options =>
                options.Filters.Add(typeof(GlobalExceptionFilter)))
                .AddNewtonsoftJson(cfg =>
                {
                    cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
                );

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                   policy =>
                   {
                       policy.WithOrigins()
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                   });

                options.AddPolicy(AppConstant.AppApiCorsPolicy, policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseAllowedHostExt(this IApplicationBuilder app, string path = "")
        {
            app.UseCors();
            app.UseCors(AppConstant.AppApiCorsPolicy);

            return app;
        }
    }
}
