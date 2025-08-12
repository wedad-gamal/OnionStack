using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;


namespace WebUI.IntegrationTests.TestFixtures
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration
                //var descriptor = services.SingleOrDefault(
                //    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                //if (descriptor != null)
                //    services.Remove(descriptor);

                //// Add ApplicationDbContext using InMemoryDatabase for testing
                //services.AddDbContext<AppDbContext>(options =>
                //{
                //    options.UseInMemoryDatabase("TestDb");
                //});

                //// Build the service provider
                //var sp = services.BuildServiceProvider();

                //// Create the database and seed test data
                //using var scope = sp.CreateScope();
                //var scopedServices = scope.ServiceProvider;
                //var db = scopedServices.GetRequiredService<AppDbContext>();
                //db.Database.EnsureCreated();

                //// Optionally seed initial test data here
                //// SeedTestData(db);
            });
        }
    }
}
