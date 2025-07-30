namespace Core.Interfaces
{
    public interface IResetPasswordUrlGenerator
    {
        string GenerateUrl(string email, string token);
    }
}
