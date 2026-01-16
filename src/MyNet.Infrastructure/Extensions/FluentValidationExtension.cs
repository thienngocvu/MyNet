using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Application.Mappings;
using System.Reflection;

namespace MyNet.Infrastructure.Extensions
{
    public static class FluentValidationExtension
    {
        public static IServiceCollection AddFluentValidationExt(this IServiceCollection services)
        {
            // Assuming validators are in Application layer, we pick one type to locate assembly
            // Currently no validators created, but we can use UserProfile as a marker for Application assembly
            services.AddValidatorsFromAssembly(typeof(UserProfile).Assembly);
            
            return services;
        }
    }
}
