namespace Abstraction.Abstraction.Interfaces.Services
{
    public interface IUrlGenerator
    {
        string GenerateUrl(string email, string token, string action, string controller);
    }
}
