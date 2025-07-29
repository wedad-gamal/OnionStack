namespace Web.Controllers;

public class HomeController : Controller
{

    private readonly ILoggerService _structuredLogger;
    private readonly IEmployeeService _employeeService;

    public HomeController(ILoggerService structuredLogger, IEmployeeService employeeService)
    {

        _structuredLogger = structuredLogger;
        _employeeService = employeeService;
    }

    public IActionResult Index()
    {
        _structuredLogger.Info("entered index page");
        return View();
    }

    [HttpGet("boarding-job/{id:int}")]
    public IActionResult BoardingJob(int id)
    {
        _structuredLogger.Info("boarding job");
        _employeeService.ScheduleOnboarding(id);
        return Ok("job is done");
    }




    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
