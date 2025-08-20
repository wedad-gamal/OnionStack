using Application.DTOs.Identity;

namespace Infrastructure.Extensions
{
    public static class IdentityResultExtensions
    {
        public static IdentityResultDto ToDto(this IdentityResult result) => new()
        {
            Succeeded = result.Succeeded,
            Errors = result.Errors.Select(e => e.Description).ToList()
        };
    }
}
