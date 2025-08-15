using Application.Common.Interfaces.Background;
using Infrastructure.BackgroundJobs;
using Moq;
using System.Linq.Expressions;

namespace Infrastructure.IntegrationTests.Background
{

    public class BackgroundJobServiceTests
    {
        private readonly Mock<IHangfireClient> _hangfireClientMock;
        private readonly BackgroundJobService _backgroundJobService;

        public BackgroundJobServiceTests()
        {
            _hangfireClientMock = new Mock<IHangfireClient>();
            _backgroundJobService = new BackgroundJobService(_hangfireClientMock.Object);
        }

        [Fact]
        public void Enqueue_ShouldCallClientEnqueue_ForAsync()
        {
            // Arrange
            _hangfireClientMock.Setup(c => c.Enqueue<IEmailJob>(It.IsAny<Expression<Func<IEmailJob, Task>>>())).Returns("jobId");

            // Act
            _backgroundJobService.EnqueueSendRoleChangedEmail("1", "Admin", true);

            // Assert
            _hangfireClientMock.Verify(c => c.Enqueue<IEmailJob>(It.IsAny<Expression<Func<IEmailJob, Task>>>()), Times.Once);
        }

        //[Fact]
        //public void Schedule_ShouldCallClientSchedule_ForAsync()
        //{
        //    // Arrange
        //    _clientMock.Setup(c => c.Schedule<IEmailJob>(It.IsAny<Expression<Func<IEmailJob, Task>>>(), It.IsAny<TimeSpan>())).Returns("jobId");

        //    // Act
        //    var result = _service.Schedule<IEmailJob>(job => job.SendRoleChangedEmailAsync("1", "Admin"), TimeSpan.FromMinutes(10));

        //    // Assert
        //    _clientMock.Verify(c => c.Schedule<IEmailJob>(It.IsAny<Expression<Func<IEmailJob, Task>>>(), TimeSpan.FromMinutes(10)), Times.Once);
        //}

        //[Fact]
        //public void AddOrUpdateRecurring_ShouldCallClientRecurring_ForAsync()
        //{
        //    // Act
        //    _service.AddOrUpdateRecurring<IEmailJob>("daily-job", job => job.SendRoleChangedEmailAsync("1", "Admin"), "0 9 * * *");

        //    // Assert
        //    _clientMock.Verify(c => c.AddOrUpdateRecurring<IEmailJob>("daily-job", It.IsAny<Expression<Func<IEmailJob, Task>>>(), "0 9 * * *"), Times.Once);
        //}
        [Fact]
        public void EnqueueSendRoleChangedEmail_ShouldCallHangfireClient()
        {
            // Arrange
            var hangfireClientMock = new Mock<IHangfireClient>();
            var backgroundJobService = new BackgroundJobService(hangfireClientMock.Object);

            var email = "test@gmail.com";
            var testRoleName = "Admin";
            var isAssigned = true;

            // Act
            backgroundJobService.EnqueueSendRoleChangedEmail(email, testRoleName, isAssigned);

            // Assert
            hangfireClientMock.Verify(client =>
                client.Enqueue<IEmailJob>(job => job.SendRoleChangedEmailAsync(email, testRoleName, isAssigned)),
                Times.Once);
        }


        [Fact]
        public void EnqueueSendPasswordResetEmail_ShouldCallHangfireClient()
        {
            var email = "test@example.com";
            var token = "token";
            _backgroundJobService.EnqueueSendPasswordResetEmail(email, token);
            _hangfireClientMock.Verify(client =>
                client.Enqueue<IEmailJob>(It.IsAny<Expression<Action<IEmailJob>>>()),
                Times.Once);
        }

        [Theory]
        [InlineData(null, "Admin", true)]
        [InlineData(null, "Admin", false)]
        [InlineData("", "Admin", true)]
        [InlineData("", "Admin", false)]
        [InlineData("test@gmail.com", null, true)]
        [InlineData("test@gmail.com", "", false)]
        public void EnqueueSendRoleChangedEmail_InvalidArgs_ShouldThrow(string email, string roleName, bool isAssigned)
        {
            Assert.Throws<ArgumentException>(() =>
                _backgroundJobService.EnqueueSendRoleChangedEmail(email, roleName, isAssigned));
            _hangfireClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void MultipleEnqueueCalls_ShouldAllBeQueued()
        {
            var user1 = "u1";
            var user2 = "u2";

            _backgroundJobService.EnqueueSendWelcomeEmail(user1);
            _backgroundJobService.EnqueueSendWelcomeEmail(user2);

            _hangfireClientMock.Verify(client => client.Enqueue<IEmailJob>(It.IsAny<Expression<Action<IEmailJob>>>()), Times.Exactly(2));
        }
    }

}
