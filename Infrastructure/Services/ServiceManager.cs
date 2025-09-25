namespace Infrastructure.Services;
internal class ServiceManager : IServiceManager
{
    public ServiceManager(
        IAppUserService appUserService,
        IAccountService accountService,
        IEmailService emailService,
        ISmsService smsService,
        INotificationService notificationService,
        IRoleService roleService,
        IEmployeeService employeeService)
    {
        AppUserService = appUserService;
        AccountService = accountService;
        EmailService = emailService;
        SmsService = smsService;
        NotificationService = notificationService;
        RoleService = roleService;
        EmployeeService = employeeService;
    }

    public IAppUserService AppUserService { get; }
    public IAccountService AccountService { get; }
    public IEmailService EmailService { get; }
    public ISmsService SmsService { get; }
    public INotificationService NotificationService { get; }
    public IRoleService RoleService { get; }
    public IEmployeeService EmployeeService { get; }
}
