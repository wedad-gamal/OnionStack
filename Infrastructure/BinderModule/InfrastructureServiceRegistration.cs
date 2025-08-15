using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Repositories;

namespace Infrastructure.BinderModule
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {


            services.AddSignalR();
            services.AddMediatR(typeof(ChangeRoleHandler).Assembly);

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            MapsterConfig.RegisterMappings();
            services.AddMapster();

            var configInstance = TypeAdapterConfig.GlobalSettings;
            configInstance.Scan(typeof(AssemblyMarker).Assembly);
            services.AddSingleton(configInstance);
            //

            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();



            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAppUserManager, AppUserManager>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<INotificationService, NotificationService>();

            //// 📬 MailKit setup
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            // Bind Twilio settings
            services.Configure<TwilioSettings>(config.GetSection("TwilioSettings"));
            services.AddTransient<ISmsService, WhatsAppService>();
            //services.AddKeyedScoped<ISmsService, Services.WhatsAppService>("whatsapp");
            //services.AddKeyedScoped<ISmsService, Services.SmsService>("sms");

            //// 🧵 Hangfire setup
            services.AddHangfire(x => x.UseSqlServerStorage(config.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();



            // Register background job service
            services.AddScoped<IHangfireClient, HangfireClient>();
            services.AddScoped<IBackgroundJobService, BackgroundJobService>();
            // Register jobs
            services.AddScoped<IEmailJob, EmailJob>();
            services.AddScoped<IOnboardingJob, OnboardingJob>();
            return services;
        }
    }
}
