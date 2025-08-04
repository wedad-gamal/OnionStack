using Infrastructure.BackgroundJobs;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.BinderModule
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Add EF DbContext, Identity, Email, File Storage, etc.



            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            var configInstance = TypeAdapterConfig.GlobalSettings;
            configInstance.Scan(typeof(AssemblyMarker).Assembly); // Infrastructure
            services.AddSingleton(configInstance);

            services.AddHttpContextAccessor();
            services.AddSingleton<ICorrelationIdAccessor, CorrelationIdAccessor>();

            services.AddScoped<ILoggerManager, LoggerManager>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();


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
