using MVC.ViewModels;

namespace Web.Controllers;

public class HomeController : Controller
{

    private readonly ILoggerManager _structuredLogger;
    private readonly IServiceManager _serviceManager;

    public HomeController(ILoggerManager structuredLogger, IServiceManager serviceManager)
    {

        _structuredLogger = structuredLogger;
        _serviceManager = serviceManager;
    }

    public IActionResult Index()
    {
        var transactions = new List<TransactionViewModel>()
        {
            new (){
                Id = "88338",
                Name = "trnx - 1",
                CreatedDate = DateTime.Now,
            },
            new (){
                Id = "89383",
                Name = "trnx - 2",
                CreatedDate = DateTime.Now,
            },
            new (){
                Id = "89939383",
                Name = "trnx - 3",
                CreatedDate = DateTime.Now,
            }
        };
        return View(transactions);
    }

    [HttpGet("boarding-job/{id:int}")]
    public IActionResult BoardingJob(int id)
    {
        _structuredLogger.Info("boarding job");
        _serviceManager.EmployeeService.ScheduleOnboarding(id);
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
    public IActionResult testimonial()
    {
        return PartialView("_TestimonialPartial");
    }
    public IActionResult Transactions()
    {

        return PartialView("_TransactionsPartial");
    }
}
