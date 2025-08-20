using Application.DTOs.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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
                 src.Succeeded ? SignInResult.Success : src.IsLockedOut ? SignInResult.LockedOut
                                : src.IsNotAllowed ? SignInResult.NotAllowed
                                : src.RequiresTwoFactor ? SignInResult.TwoFactorRequired
                                : SignInResult.Failed

                );

            // =============================
            // ExternalLoginInfo ↔ ExternalLoginInfoDto
            // =============================
            config.NewConfig<ExternalLoginInfo, ExternalLoginInfoDto>()
                .Map(dest => dest.LoginProvider, src => src.LoginProvider)
                .Map(dest => dest.ProviderKey, src => src.ProviderKey)
                .Map(dest => dest.ProviderDisplayName, src => src.ProviderDisplayName)
                .Map(dest => dest.DisplayName, src => src.Principal.Identity != null ? src.Principal.Identity.Name : null);



            config
                .NewConfig<ExternalLoginInfoDto, ExternalLoginInfo>()
                .ConstructUsing(src =>
                    new ExternalLoginInfo(
                        new ClaimsPrincipal(new ClaimsIdentity(
                            !string.IsNullOrEmpty(src.DisplayName)
                                ? new[] { new Claim(ClaimTypes.Name, src.DisplayName) }
                                : Array.Empty<Claim>())),
                        src.LoginProvider ?? string.Empty,
                        src.ProviderKey ?? string.Empty,
                        src.ProviderDisplayName ?? src.DisplayName // fallback if ProviderDisplayName is null
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
        }
    }
}
