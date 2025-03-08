using Microsoft.AspNetCore.Mvc;

namespace The_Glamour.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Appointments()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }
    }
}
