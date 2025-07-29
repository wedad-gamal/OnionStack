namespace Infrastructure.BackgroundJobs
{
    public class OnboardingJob : IOnboardingJob
    {
        private readonly IEmailService _emailSender;
        private readonly IEmployeeRepository _repo;
        private readonly ILoggerService _logger;
        private readonly ICorrelationIdContext _correlationIdContext;

        public OnboardingJob(IEmailService emailSender, IEmployeeRepository repo, ILoggerService logger, ICorrelationIdContext correlationIdContext)
        {
            _emailSender = emailSender;
            _repo = repo;
            _logger = logger;
            _correlationIdContext = correlationIdContext;
        }

        public async Task RunAsync(int employeeId)
        {
            var correlationId = Guid.NewGuid().ToString();
            correlationId = _correlationIdContext.CorrelationId ?? correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                _logger.Info("Starting onboarding for employee {EmployeeId}", employeeId);

                //var employee = await _repo.GetByIdAsync(employeeId);
                var employee = new Employee() { Id = 1, FirstName = "wedad", LastName = "gamal", Email = "wedad.gamal@gmail.com" };
                if (employee == null)
                {
                    _logger.Warn("Employee with ID {EmployeeId} not found", employeeId);
                    throw new Exception();
                }

                await _emailSender.SendEmailAsync(employee.Email, "Welcome!", "...");
                _logger.Info("Welcome email sent to {Email}", employee.Email);

            }

        }
    }
}
