using Application.Abstraction.Interfaces.Common;
using Application.Abstraction.Interfaces.Services;

namespace Infrastructure.Services.Common;
internal class ServiceManager : IServiceManager
{
    public ServiceManager(
        IAppUserService appUserService,
        IAccountService accountService,
        IEmailService emailService,
        ISmsService smsService,
        INotificationService notificationService,
        IRoleService roleService,
        IEmployeeService employeeService,
        ICategoryService categoryService)
    {
        AppUserService = appUserService;
        AccountService = accountService;
        EmailService = emailService;
        SmsService = smsService;
        NotificationService = notificationService;
        RoleService = roleService;
        EmployeeService = employeeService;
        CategoryService = categoryService;
    }

    public IAppUserService AppUserService { get; }
    public IAccountService AccountService { get; }
    public IEmailService EmailService { get; }
    public ISmsService SmsService { get; }
    public INotificationService NotificationService { get; }
    public IRoleService RoleService { get; }
    public IEmployeeService EmployeeService { get; }
    public ICategoryService CategoryService { get; }
}
