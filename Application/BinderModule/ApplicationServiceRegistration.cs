using Application.Common.Mapping;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.BinderModule
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add MediatR, services, validators, etc.


            MapsterConfig.RegisterMappings();
            services.AddMapster();

            var configInstance = TypeAdapterConfig.GlobalSettings;
            configInstance.Scan(typeof(AssemblyMarker).Assembly); // Application
            services.AddSingleton(configInstance);

            return services;
        }
    }
}
