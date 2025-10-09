namespace Application.Abstraction.Interfaces.Common
{
    public interface IUrlGenerator
    {
        string GenerateUrl(string email, string token, string action, string controller);
    }
}
