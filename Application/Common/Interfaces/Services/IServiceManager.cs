using Application.Common.Interfaces.Identity;

namespace Application.Common.Interfaces.Services;
public interface IServiceManager
{
    IAppUserService AppUserService { get; }
    IAccountService AccountService { get; }
    IEmailService EmailService { get; }
    ISmsService SmsService { get; }
    INotificationService NotificationService { get; }
    IRoleService RoleService { get; }

    //

    IEmployeeService EmployeeService { get; }
}
