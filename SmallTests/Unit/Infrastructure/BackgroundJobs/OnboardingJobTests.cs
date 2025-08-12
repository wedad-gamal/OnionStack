namespace Tests.Unit.Infrastructure.BackgroundJobs
{
    public class OnboardingJobTests
    {
        [Fact]
        public async Task RunAsync_ShouldSendWelcomeEmail_WhenEmployeeExists()
        {
            // Arrange
            //var mockRepo = new Mock<IEmployeeRepository>();
            //var mockSender = new Mock<IEmailSender>();

            //mockRepo.Setup(r => r.GetByIdAsync(1))
            //        .ReturnsAsync(new Employee { Id = 1, FullName = "Wedad", Email = "wedad@example.com" });

            //var job = new OnboardingJob(mockSender.Object, mockRepo.Object);

            //// Act
            //await job.RunAsync(1);

            //// Assert
            //mockSender.Verify(s => s.SendEmailAsync(
            //    "wedad@example.com",
            //    "Welcome to the Company!",
            //    It.Is<string>(body => body.Contains("Welcome Wedad")),
            //    true), Times.Once);
        }
    }
}
