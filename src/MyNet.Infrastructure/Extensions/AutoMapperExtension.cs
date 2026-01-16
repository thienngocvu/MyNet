using Microsoft.Extensions.DependencyInjection;
using MyNet.Application.Mappings;

namespace MyNet.Infrastructure.Extensions
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapperExt(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => {}, typeof(UserProfile).Assembly);
            return services;
        }
    }
}
