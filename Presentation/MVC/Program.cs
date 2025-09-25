using CorrelationId;
using Infrastructure.RealTime;
using MVC.Middleware;

namespace Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();
        builder.Services.AddPresentationServices(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Configuration.AddUserSecrets<Program>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        // Use Correlation ID middleware early in pipeline
        app.UseCorrelationId();
        app.UseSerilogRequestLogging();
        app.MapHub<NotificationHub>("/notificationHub");

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHangfireDashboard("/hangfire");

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
