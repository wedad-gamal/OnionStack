namespace Application.Abstractions.UrlGeneration
{
    public interface IResetPasswordUrlGenerator
    {
        string GenerateUrl(string email, string token);
    }
}
