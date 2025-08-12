namespace Application.Common.Interfaces.Background
{
    public interface IOnboardingJob
    {
        Task RunAsync(int employeeId);
    }
}
