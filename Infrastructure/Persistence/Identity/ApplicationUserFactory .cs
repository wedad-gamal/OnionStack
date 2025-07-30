using Application.Abstractions.Identity;
using Core.Interfaces.Identity;

namespace Infrastructure.Persistence.Identity
{
    public class ApplicationUserFactory : IApplicationUserFactory
    {
        public IApplicationUser Create() => new ApplicationUser();
    }
}
