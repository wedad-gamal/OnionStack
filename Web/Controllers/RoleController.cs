using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
