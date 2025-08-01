using Infrastructure.BackgroundJobs;
using Infrastructure.Context;
using Infrastructure.Email;
using Infrastructure.Logging;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<ILoggerManager, LoggerManager>();


            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoleService, RoleService>();


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
