using Application.DTOs.Common;
using Microsoft.AspNetCore.Authentication;

namespace Infrastructure.Mapping
{
    public class IdentityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // =============================
            // IdentityResult ↔ IdentityResultDto
            // =============================
            config.NewConfig<IdentityResult, IdentityResultDto>()
                .Map(dest => dest.Succeeded, src => src.Succeeded)
                .Map(dest => dest.Errors, src => src.Errors.Select(e => e.Description));

            config.NewConfig<IdentityResultDto, IdentityResult>()
                .ConstructUsing(src =>
                    src.Succeeded
                        ? IdentityResult.Success
                        : IdentityResult.Failed(src.Errors.Select(e => new IdentityError { Description = e }).ToArray())
                );

            // =============================
            // SignInResult ↔ SignInResultDto
            // =============================
            config.NewConfig<SignInResult, SignInResultDto>()
                .Map(dest => dest.Succeeded, src => src.Succeeded)
                .Map(dest => dest.IsLockedOut, src => src.IsLockedOut)
                .Map(dest => dest.IsNotAllowed, src => src.IsNotAllowed)
                .Map(dest => dest.RequiresTwoFactor, src => src.RequiresTwoFactor);

            config.NewConfig<SignInResultDto, SignInResult>()
                .ConstructUsing(src =>
                    src.Succeeded ? SignInResult.Success :
                    src.IsLockedOut ? SignInResult.LockedOut :
                    src.IsNotAllowed ? SignInResult.NotAllowed :
                    src.RequiresTwoFactor ? SignInResult.TwoFactorRequired :
                    SignInResult.Failed
                );

            // =============================
            // ExternalLoginInfo ↔ ExternalLoginInfoDto
            // =============================
            config.NewConfig<ExternalLoginInfo, ExternalLoginInfoDto>()
                .Map(dest => dest.Principal, src => src.Principal)
                .Map(dest => dest.LoginProvider, src => src.LoginProvider)
                .Map(dest => dest.ProviderKey, src => src.ProviderKey)
                .Map(dest => dest.ProviderDisplayName, src => src.ProviderDisplayName)
                .Map(dest => dest.DisplayName,
                     src => src.Principal.Identity != null ? src.Principal.Identity.Name : null);

            config.NewConfig<ExternalLoginInfoDto, ExternalLoginInfo>()
                .ConstructUsing(src =>
                    new ExternalLoginInfo(
                        src.Principal ?? new ClaimsPrincipal(new ClaimsIdentity(
                            !string.IsNullOrEmpty(src.DisplayName)
                                ? new[] { new Claim(ClaimTypes.Name, src.DisplayName) }
                                : Array.Empty<Claim>()
                        )),
                        src.LoginProvider ?? string.Empty,
                        src.ProviderKey ?? string.Empty,
                        src.ProviderDisplayName ?? src.DisplayName
                    )
                );

            // =============================
            // AuthenticationProperties ↔ AuthenticationPropertiesDto
            // =============================
            config.NewConfig<AuthenticationProperties, AuthenticationPropertiesDto>()
                .Map(dest => dest.Items, src => src.Items)
                .Map(dest => dest.AllowRefresh, src => src.AllowRefresh ?? false)
                .Map(dest => dest.ExpiresUtc, src => src.ExpiresUtc)
                .Map(dest => dest.IssuedUtc, src => src.IssuedUtc)
                .Map(dest => dest.IsPersistent, src => src.IsPersistent)
                .Map(dest => dest.RedirectUri, src => src.RedirectUri);

            config.NewConfig<AuthenticationPropertiesDto, AuthenticationProperties>()
                .ConstructUsing(src => new AuthenticationProperties(src.Items)
                {
                    AllowRefresh = src.AllowRefresh,
                    ExpiresUtc = src.ExpiresUtc,
                    IssuedUtc = src.IssuedUtc,
                    IsPersistent = src.IsPersistent,
                    RedirectUri = src.RedirectUri
                });

            config.NewConfig<CreateUserDto, ApplicationUser>()
                .ConstructUsing(src => new ApplicationUser(src.UserName, new Email(src.Email)))
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.ProfilePictureUrl, src => src.ProfilePictureUrl)
                .Map(dest => dest.Age, src => src.Age);

            config.NewConfig<ApplicationUser, UserDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.Email, src => src.EmailAddress.Value)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Age, src => src.Age)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.ProfilePictureUrl, src => src.ProfilePictureUrl);
        }
    }
}
