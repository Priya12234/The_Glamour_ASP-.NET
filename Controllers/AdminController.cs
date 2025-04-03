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
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Feedback()
        {
            return View();
        }
    }
}