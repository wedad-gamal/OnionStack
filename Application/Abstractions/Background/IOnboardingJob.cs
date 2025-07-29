namespace Application.Abstractions.Background
{
    public interface IOnboardingJob
    {
        Task RunAsync(int employeeId);
    }
}
