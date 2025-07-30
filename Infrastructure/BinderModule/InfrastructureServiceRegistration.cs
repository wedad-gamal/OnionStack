using Application.Abstractions.Identity;
using Core.Interfaces.Identity;
using Hangfire;
using Infrastructure.BackgroundJobs;
using Infrastructure.Context;
using Infrastructure.Email;
using Infrastructure.Logging;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;

namespace Infrastructure.BinderModule
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Add EF DbContext, Identity, Email, File Storage, etc.


            services.AddScoped<IApplicationUser, ApplicationUser>();
            services.AddScoped<IAppUserManager, AppUserManager>();


            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()  // use your custom DbContext
                .AddDefaultTokenProviders();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IApplicationUserFactory, ApplicationUserFactory>();
            services.AddScoped<ILoggerService, SerilogLoggerService>();
            services.AddSingleton<ICorrelationIdContext, CorrelationIdContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILogEventEnricher>(provider =>
            {
                var context = provider.GetRequiredService<ICorrelationIdContext>();
                return new CorrelationIdEnricher(context);
            });

            //// 📬 MailKit setup
            services.Configure<EmailSettings>(config.GetSection("MailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            //// 🧵 Hangfire setup
            services.AddHangfire(x => x.UseSqlServerStorage(config.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            services.AddScoped<IOnboardingJob, OnboardingJob>();
            //services.AddScoped<IEmailJob, EmailJob>();

            return services;
        }
    }
}
