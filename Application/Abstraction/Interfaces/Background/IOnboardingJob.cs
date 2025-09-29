namespace Abstraction.Abstraction.Interfaces.Background
{
    public interface IOnboardingJob
    {
        Task RunAsync(int employeeId);
    }
}
