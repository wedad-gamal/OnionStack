

using Application.Abstractions.Background;

namespace Application.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IOnboardingJob _onboardingJob;

        public EmployeeService(IOnboardingJob onboardingJob)
        {
            _onboardingJob = onboardingJob;
        }

        public void ScheduleOnboarding(int employeeId)
        {
            BackgroundJob.Enqueue(() => _onboardingJob.RunAsync(employeeId));
        }
    }
}
