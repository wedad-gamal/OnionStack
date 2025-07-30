using Application.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.BinderModule
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add MediatR, services, validators, etc.


            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }
    }
}
