

using MVC.Apis.Filters;
using MVC.Filters;

namespace Web.BinderModule;

public static class PresentationServiceRegistration
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<CustomExceptionFilter>();
        });
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiResponseFilter>();
        });
        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "XSRF-TOKEN"; // Angular expects this name
            options.HeaderName = "X-XSRF-TOKEN"; // Angular sends this header
            options.HeaderName = "X-CSRF-TOKEN"; // Custom header name for AJAX

        });

        // Add Correlation ID services
        services.AddDefaultCorrelationId(options =>
        {
            options.AddToLoggingScope = true;
            options.EnforceHeader = false; // allow auto-generation if client doesn't send one
            options.IgnoreRequestHeader = false;
            options.RequestHeader = "X-Correlation-ID";
            options.ResponseHeader = "X-Correlation-ID";
            options.UpdateTraceIdentifier = true;
            options.IncludeInResponse = true;
        });
        Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .Enrich.WithCorrelationId()
               .Enrich.WithThreadId()
               .Enrich.WithEnvironmentUserName()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .Enrich.WithCorrelationIdHeader("X-Correlation-ID") // or your preferred header
               .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [CID:{CorrelationId}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [CID:{CorrelationId}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Seq("http://localhost:5341") // seq
               .CreateLogger();

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

        services.AddHttpContextAccessor(); // Needed for IHttpContextAccessor
        services.AddScoped<ILoggerManager, LoggerManager>();
        services.AddScoped<IUrlGenerator, UrlGenerator>();

        //fluent validators
        services.AddValidatorsFromAssembly(typeof(Application.AssemblyMarker).Assembly); // Register all validators from the Application layer
        services.AddFluentValidationAutoValidation(); // enables model validation
        services.AddFluentValidationClientsideAdapters(); // optional for client-side

        services.AddControllersWithViews();

        return services;
    }
}
