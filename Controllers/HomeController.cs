using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace The_Glamour.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
