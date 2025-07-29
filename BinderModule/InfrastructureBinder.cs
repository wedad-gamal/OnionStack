using Application.Logging;
using Core.Identity;
using Serilog.Core;
using Shared.Logging;


namespace BinderModules
{
    public static class InfrastructureBinder
    {
        public static void Register(this IServiceCollection services, IConfiguration config)
        {

            services.AddScoped<IApplicationUser, ApplicationUser>();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()  // use your custom DbContext
                .AddDefaultTokenProviders();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IApplicationUserFactory, ApplicationUserFactory>();
            services.AddScoped<ILoggerService, SerilogLoggerService>();
            services.AddSingleton<ICorrelationIdContext, CorrelationIdContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();

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

        }
    }

}
