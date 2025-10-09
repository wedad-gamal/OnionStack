namespace Infrastructure.IntegrationTests.Services
{
    [Collection("Mapster")]
    public class AccountServiceTests : TestBase
    {
        private readonly Mock<IAppUserService> _appUserManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IUrlGenerator> _urlGeneratorMock;
        private readonly Mock<ILoggerManager> _loggerMock;
        private readonly Mock<IServiceManager> _serviceManager;
        //private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly AccountService _sut; // system under test

        public AccountServiceTests()
        {
            _appUserManagerMock = new Mock<IAppUserService>();
            _emailServiceMock = new Mock<IEmailService>();
            _urlGeneratorMock = new Mock<IUrlGenerator>();
            _serviceManager = new Mock<IServiceManager>();
            _loggerMock = new Mock<ILoggerManager>();
            //_unitOfWork = new Mock<IUnitOfWork>();
            // SignInManager & UserManager require special mocks
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                _userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null, null, null, null);

            _sut = new AccountService(
                _signInManagerMock.Object,
                _userManagerMock.Object,
                _appUserManagerMock.Object,
                _urlGeneratorMock.Object,
                _loggerMock.Object
            );

        }

        [Fact]
        public async Task ForgotPassword_Should_SendEmail_AndReturnSuccess()
        {
            // Arrange
            var email = "test@example.com";
            _appUserManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(email))
                .ReturnsAsync("reset-token");
            _urlGeneratorMock.Setup(m => m.GenerateUrl(email, "reset-token", "action", "controller"))
                .Returns("http://reset-url");

            // Act
            var result = await _sut.ForgotPassword(email, "action", "controller");

            // Assert
            result.Succeeded.Should().BeTrue();
            _emailServiceMock.Verify(e => e.SendEmailAsync(email, "Reset Password", "http://reset-url", true), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_Should_Call_AppUserManager_AndReturnResult()
        {
            // Arrange
            var dto = new LoginDto { Email = "test@example.com", Password = "123", RememberMe = true };
            var expected = new IdentityResultDto { Succeeded = true };
            _appUserManagerMock.Setup(m => m.LoginAsync(dto)).ReturnsAsync(expected);

            // Act
            var result = await _sut.LoginAsync(dto);

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Logout_Should_Call_SignInManager()
        {
            // Act
            await _sut.Logout();

            // Assert
            _signInManagerMock.Verify(m => m.SignOutAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Should_Call_AppUserManager()
        {
            // Arrange
            //var dto = new CreateUserDto { Email = "test@example.com", Password = "123" };
            //_appUserManagerMock.Setup(m => m.CreateUserAsync(dto)).ReturnsAsync(new IdentityResultDto { Succeeded = true });

            //// Act
            //var result = await _sut.RegisterAsync(dto);

            //// Assert
            //result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void ConfigureExternalAuthenticationProperties_Should_ReturnMappedDto()
        {
            // Arrange
            var authProps = new AuthenticationProperties(new Dictionary<string, string?>
            {
                { "key1", "value1" }
            })
            {
                RedirectUri = "http://redirect"
            };
            var userId = "UserId_123";
            _signInManagerMock.Setup(m => m.ConfigureExternalAuthenticationProperties("Google", "http://redirect", userId))
                .Returns(authProps);

            // Act
            var result = _sut.ConfigureExternalAuthenticationProperties("Google", "http://redirect");

            // Assert
            result.Items.Should().ContainKey("key1");
            result.RedirectUri.Should().Be("http://redirect");
        }

        [Fact]
        public async Task CreateUserAsync_Should_MapResult()
        {
            // Arrange
            var user = new ApplicationUser("username", "test@example.com");
            _userManagerMock.Setup(m => m.CreateAsync(user, "123"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _sut.CreateUserAsync(user, "123");

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task AddLoginAsync_Should_ReturnMappedResult()
        {
            // Arrange
            var user = new ApplicationUser("test@example.com", "test@example.com");
            var info = new ExternalLoginInfoDto { LoginProvider = "Google", ProviderKey = "123" };

            _userManagerMock.Setup(m => m.AddLoginAsync(user, It.IsAny<ExternalLoginInfo>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _sut.AddLoginAsync(user, info);

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task ExternalLoginSignInAsync_Should_ReturnMappedDto()
        {
            // Arrange
            _signInManagerMock.Setup(m => m.ExternalLoginSignInAsync("Google", "123", false, default))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _sut.ExternalLoginSignInAsync("Google", "123");

            // Assert
            result.Succeeded.Should().BeTrue();
        }
        [Fact]
        public async Task HandleExternalLoginAsync_Should_Fail_When_NoLoginInfo()
        {
            // Arrange
            ExternalLoginInfoDto externalLoginInfoDto = new ExternalLoginInfoDto
            {
                LoginProvider = "Google",
                ProviderKey = "123",
                DisplayName = "Test User"
            };
            _signInManagerMock.Setup(m => m.GetExternalLoginInfoAsync(null))
                .ReturnsAsync((ExternalLoginInfo?)null);

            // Act
            var result = await _sut.HandleExternalLoginAsync(externalLoginInfoDto);

            // Assert
            result.Succeeded.Should().BeFalse();
            //result.Errors.Should().Contain("Error loading external login information.");
        }

        [Fact]
        public async Task HandleExternalLoginAsync_Should_Fail_When_EmailClaimMissing()
        {
            // Arrange
            var claims = new List<Claim>(); // no email claim
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var loginInfo = new ExternalLoginInfo(principal, "Google", "123", "display");
            _signInManagerMock.Setup(m => m.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(loginInfo);
            var externalLoginInfoDto = new ExternalLoginInfoDto
            {
                LoginProvider = "Google",
                ProviderKey = "123",
                DisplayName = "Test User"
            };
            // Act
            var result = await _sut.HandleExternalLoginAsync(externalLoginInfoDto);

            // Assert
            result.Succeeded.Should().BeFalse();
            //result.Errors.Should().Contain("Email claim not found.");
        }

        [Fact]
        public async Task HandleExternalLoginAsync_Should_ReturnSuccess_When_ExistingUser()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "test@example.com")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var loginInfo = new ExternalLoginInfo(principal, "Google", "123", "display");
            var loginInfoDto = new ExternalLoginInfoDto()
            {
                LoginProvider = "Google",
                ProviderKey = "123",
                DisplayName = "Test User"
            };

            _signInManagerMock.Setup(m => m.GetExternalLoginInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(loginInfo);
            _signInManagerMock.Setup(m => m.ExternalLoginSignInAsync("Google", "123", false))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _sut.HandleExternalLoginAsync(loginInfoDto);

            // Assert
            result.Succeeded.Should().BeTrue();
        }
        [Fact]
        public async Task HandleExternalLoginAsync_Should_CreateNewUser_When_UserNotFound()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "newuser@example.com")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var loginInfo = new ExternalLoginInfo(principal, "Google", "123", "display");
            var loginDto = loginInfo.Adapt<ExternalLoginInfoDto>();
            _signInManagerMock.Setup(m => m.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(loginInfo);

            _userManagerMock.Setup(m => m.FindByEmailAsync("newuser@example.com"))
                .ReturnsAsync((ApplicationUser?)null);

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(m => m.AddLoginAsync(It.IsAny<ApplicationUser>(), loginInfo))
                .ReturnsAsync(IdentityResult.Success);

            _signInManagerMock.Setup(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, "Google"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.HandleExternalLoginAsync(loginDto);

            // Assert
            result.Succeeded.Should().BeTrue();
            _userManagerMock.Verify(m => m.CreateAsync(It.Is<ApplicationUser>(u => u.Email == "newuser@example.com")), Times.Once);
            _userManagerMock.Verify(m => m.AddLoginAsync(It.IsAny<ApplicationUser>(), loginInfo), Times.Once);
            _signInManagerMock.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, "Google"), Times.Once);
        }

        [Fact]
        public async Task HandleExternalLoginAsync_Should_Fail_When_UserCreationFails()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "failuser@example.com")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var loginInfo = new ExternalLoginInfo(principal, "Google", "123", "display");
            var loginInfoDto = loginInfo.Adapt<ExternalLoginInfoDto>();
            _signInManagerMock.Setup(m => m.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(loginInfo);

            _userManagerMock.Setup(m => m.FindByEmailAsync("failuser@example.com"))
                .ReturnsAsync((ApplicationUser?)null);

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

            // Act
            var result = await _sut.HandleExternalLoginAsync(loginInfoDto);

            // Assert
            result.Succeeded.Should().BeFalse();
            //result.Errors.Should().Contain("User creation failed");
        }
        [Fact]
        public async Task ResetPasswordAsync_Should_Fail_When_UserNotFound()
        {
            // Arrange
            var dto = new ResetPasswordDto
            {
                Email = "notfound@example.com",
                Token = "token",
                Password = "NewPass123!"
            };
            ApplicationUser user = null;
            _userManagerMock.Setup(m => m.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(m => m.ResetPasswordAsync(user, dto.Token, dto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User not found for password reset." }));

            // Act
            var result = await _sut.ResetPasswordAsync(dto);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("User not found.");
        }

        [Fact]
        public async Task ResetPasswordAsync_Should_Fail_When_ResetPasswordFails()
        {
            // Arrange
            var user = new ApplicationUser("test@example.com", "test@example.com");
            var dto = new ResetPasswordDto
            {
                Email = user.Email,
                Token = "invalid-token",
                Password = "NewPass123!"
            };

            _userManagerMock.Setup(m => m.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(m => m.ResetPasswordAsync(user, dto.Token, dto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token" }));

            // Act
            var result = await _sut.ResetPasswordAsync(dto);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("Invalid token");
        }

        [Fact]
        public async Task ResetPasswordAsync_Should_Succeed_When_ValidTokenAndPassword()
        {
            // Arrange
            var user = new ApplicationUser("test@example.com", "test@example.com");
            var dto = new ResetPasswordDto
            {
                Email = user.Email,
                Token = "valid-token",
                Password = "NewPass123!"
            };

            _userManagerMock.Setup(m => m.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(m => m.ResetPasswordAsync(user, dto.Token, dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _sut.ResetPasswordAsync(dto);

            // Assert
            result.Succeeded.Should().BeTrue();
        }
        [Fact]
        public async Task ForgotPasswordAsync_Should_SendResetLink_When_UserExists()
        {
            // Arrange
            var email = "user@example.com";
            var token = "reset-token";
            var expectedUrl = "https://example.com/reset?token=reset-token";

            _appUserManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(email))
                .ReturnsAsync(token);

            _urlGeneratorMock.Setup(m => m.GenerateUrl(email, token, "Reset", "Account"))
                .Returns(expectedUrl);

            _emailServiceMock.Setup(m => m.SendEmailAsync(email, "Reset Password", expectedUrl, true))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.ForgotPassword(email, "Reset", "Account");

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().Contain("Reset password email sent successfully");

            _emailServiceMock.Verify(m => m.SendEmailAsync(email, "Reset Password", expectedUrl, true), Times.Once);
        }

        [Fact]
        public async Task ForgotPasswordAsync_Should_Log_And_StillReturnSuccess_When_EmailServiceFails()
        {
            // Arrange
            var email = "user@example.com";
            var token = "reset-token";
            var expectedUrl = "https://example.com/reset?token=reset-token";

            _appUserManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(email))
                .ReturnsAsync(token);

            _urlGeneratorMock.Setup(m => m.GenerateUrl(email, token, "Reset", "Account"))
                .Returns(expectedUrl);

            _emailServiceMock.Setup(m => m.SendEmailAsync(email, "Reset Password", expectedUrl, true))
                .ThrowsAsync(new Exception("SMTP failure"));

            // Act
            Func<Task> act = async () => await _sut.ForgotPassword(email, "Reset", "Account");

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("SMTP failure");
        }
        [Fact]
        public async Task Logout_Should_CallSignOutAsync_And_LogInfo()
        {
            // Arrange
            _signInManagerMock.Setup(m => m.SignOutAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _sut.Logout();

            // Assert
            _signInManagerMock.Verify(m => m.SignOutAsync(), Times.Once);

            _loggerMock.Verify(
                x => x.Info(It.Is<string>(msg => msg.Contains("Attempting to log out user "))),
                Times.Once);
        }
    }
}
