


namespace Web.BinderModule;

public static class PresentationServiceRegistration
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddScoped<LoggingFilter, LoggingFilter>();

        services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add(typeof(LoggingFilter));
        });

        services.AddHttpContextAccessor(); // Needed for IHttpContextAccessor

        services.AddScoped<IResetPasswordUrlGenerator, ResetPasswordUrlGenerator>();

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
