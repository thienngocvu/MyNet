namespace MyNet.API.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerExt(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new()
                {
                    Title = "MyNet API",
                    Version = "v1",
                    Description = "APIs for MyNet application"
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerExt(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyNet API");
                c.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}
