using Application.Abstraction.Interfaces.Common;
using Application.Abstraction.Interfaces.Services;
using Infrastructure.Services.Common;
using Infrastructure.Services.Logging;

namespace Infrastructure.BinderModule;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        // 📦 DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
                   .UseLazyLoadingProxies()
        );

        // 🔑 Identity
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // 🔔 SignalR
        services.AddSignalR();

        // 📬 MediatR
        services.AddMediatR(typeof(ChangeRoleHandler).Assembly);

        // 🗺️ Mapster
        MapsterConfig.RegisterMappings();
        services.AddMapster();
        var configInstance = TypeAdapterConfig.GlobalSettings;
        configInstance.Scan(typeof(AssemblyMarker).Assembly);
        services.AddSingleton(configInstance);

        // 🛠️ Infrastructure cross-cutting
        services.AddHttpContextAccessor();
        services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();
        services.AddScoped<IErrorLogService, ErrorLogService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // 📌 Core services
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddTransient<ISmsService, WhatsAppService>();
        services.AddScoped<ICategoryService, CategoryService>();

        // 👑 Service aggregator
        services.AddScoped<IServiceManager, ServiceManager>();

        // 📬 MailKit setup
        services.Configure<EmailSettings>(config.GetSection("EmailSettings"));

        // 📱 Twilio setup
        services.Configure<TwilioSettings>(config.GetSection("TwilioSettings"));
        //services.AddKeyedScoped<ISmsService, Services.WhatsAppService>("whatsapp");
        //services.AddKeyedScoped<ISmsService, Services.SmsService>("sms");

        // 🧵 Hangfire
        services.AddHangfire(x => x.UseSqlServerStorage(config.GetConnectionString("HangfireConnection")));
        services.AddHangfireServer();

        // Background jobs
        services.AddScoped<IHangfireClient, HangfireClient>();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        services.AddScoped<IEmailJob, EmailJob>();
        services.AddScoped<IOnboardingJob, OnboardingJob>();

        return services;
    }
}
