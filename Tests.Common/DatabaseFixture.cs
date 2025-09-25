using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Common
{
    public class DatabaseFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }

        public DatabaseFixture()
        {
            var services = new ServiceCollection();

            // Use InMemory provider for tests
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDb-" + Guid.NewGuid()));

            // Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var provider = services.BuildServiceProvider();

            Context = provider.GetRequiredService<ApplicationDbContext>();
            UserManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            Context.Database.EnsureCreated();

            SeedData().GetAwaiter().GetResult();
        }

        private async Task SeedData()
        {
            // Ensure roles exist
            if (!await RoleManager.RoleExistsAsync("Admin"))
                await RoleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await RoleManager.RoleExistsAsync("User"))
                await RoleManager.CreateAsync(new IdentityRole("User"));

            // Seed Admin user
            if (await UserManager.FindByEmailAsync("admin@test.com") == null)
            {
                var admin = new ApplicationUser("admin@test.com", "admin@test.com");
                admin.EmailConfirmed = true;

                await UserManager.CreateAsync(admin, "Admin123!");
                await UserManager.AddToRoleAsync(admin, "Admin");
            }

            // Seed normal User
            if (await UserManager.FindByEmailAsync("user@test.com") == null)
            {
                var user = new ApplicationUser("user@test.com", "user@test.com");
                user.EmailConfirmed = true;

                await UserManager.CreateAsync(user, "User123!");
                await UserManager.AddToRoleAsync(user, "User");
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }

    [CollectionDefinition("Database")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }
}
