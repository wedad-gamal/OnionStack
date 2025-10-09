using Abstraction.Abstraction.Interfaces.Identity;
using Application.Abstraction.Interfaces.Services;

namespace Application.Abstraction.Interfaces.Common;
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
    ICategoryService CategoryService { get; }
}
