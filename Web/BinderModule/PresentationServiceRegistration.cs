

using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.BinderModule;

public static class PresentationServiceRegistration
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration config)
    {

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.ExpireTimeSpan = TimeSpan.FromDays(3); // Set the cookie expiration time
            options.AccessDeniedPath = "/Account/AccessDenied";
        })
        .AddGoogle(googleOptions =>
        {
            IConfiguration configuration = config.GetSection("Authentication:Google");
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Google authentication configuration is missing.");
            }
            if (string.IsNullOrEmpty(configuration["ClientId"]) || string.IsNullOrEmpty(configuration["ClientSecret"]))
            {
                throw new ArgumentException("Google ClientId and ClientSecret must be provided in the configuration.");
            }

            googleOptions.ClientId = configuration["ClientId"];
            googleOptions.ClientSecret = configuration["ClientSecret"];
        });

        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddScoped<LoggingFilter, LoggingFilter>();

        services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add(typeof(LoggingFilter));
        });

        services.AddHttpContextAccessor(); // Needed for IHttpContextAccessor

        services.AddScoped<IUrlGenerator, UrlGenerator>();

        //fluent validators
        services.AddValidatorsFromAssembly(typeof(Application.AssemblyMarker).Assembly); // Register all validators from the Application layer
        services.AddFluentValidationAutoValidation(); // enables model validation
        services.AddFluentValidationClientsideAdapters(); // optional for client-side

        Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentUserName()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationIdHeader("X-Correlation-ID") // or your preferred header
                .WriteTo.File("Logs/hrsystem-log-.txt", rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] ({CorrelationId}) {Message:lj}{NewLine}{Exception}")
                .WriteTo.Seq("http://localhost:5341") // seq

                .CreateLogger();

        return services;
    }
}
