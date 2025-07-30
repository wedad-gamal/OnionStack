using Core.Interfaces.Identity;

namespace Application.Abstractions.Identity
{
    public interface IApplicationUserFactory
    {
        IApplicationUser Create();
    }
}
