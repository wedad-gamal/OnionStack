using Application.Abstractions.Services;
using Infrastructure.Persistence.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Tests.Unit.Infrastructure.Services
{
    public class AccountServicesTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<ILogger<AccountService>> _loggerMock;
        private readonly IAccountService _service;

        public AccountServicesTests()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                _userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null, null, null, null);

            _loggerMock = new Mock<ILogger<AccountService>>();
            // _service = new AccountService(_userManagerMock.Object, _signInManagerMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task HandleExternalLoginAsync_CreatesUserAndSignsIn_WhenUserDoesNotExist()
        {
            var email = "test@example.com";
            var provider = "Google";
            var providerKey = "google-123";

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, email) };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var info = new ExternalLoginInfo(principal, provider, providerKey, provider);

            _signInManagerMock.Setup(m => m.ExternalLoginSignInAsync(provider, providerKey, false))
                .ReturnsAsync(SignInResult.Failed);

            _userManagerMock.Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser)null);

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(m => m.AddLoginAsync(It.IsAny<ApplicationUser>(), info))
                .ReturnsAsync(IdentityResult.Success);

            //_signInManagerMock.Setup(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false))
            //    .Returns(Task.CompletedTask);

            var result = await _service.HandleExternalLoginAsync(info);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task HandleExternalLoginAsync_ReturnsFailed_WhenUserCreationFails()
        {
            var email = "fail@example.com";
            var info = new ExternalLoginInfo(
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) })),
                "Google", "google-456", "Google");

            _signInManagerMock.Setup(m => m.ExternalLoginSignInAsync("Google", "google-456", false))
                .ReturnsAsync(SignInResult.Failed);

            _userManagerMock.Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser)null);

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Email already exists" }));

            var result = await _service.HandleExternalLoginAsync(info);

            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task HandleExternalLoginAsync_ReturnsFailed_WhenAddLoginFails()
        {
            var email = "linkfail@example.com";
            var user = new ApplicationUser { Email = email, UserName = email };
            var info = new ExternalLoginInfo(
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) })),
                "Google", "google-789", "Google");

            _signInManagerMock.Setup(m => m.ExternalLoginSignInAsync("Google", "google-789", false))
                .ReturnsAsync(SignInResult.Failed);

            _userManagerMock.Setup(m => m.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(m => m.AddLoginAsync(user, info))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Login already linked" }));

            var result = await _service.HandleExternalLoginAsync(info);

            Assert.False(result.Succeeded);
        }
    }

}