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
        // Action for Appointment Form
        public IActionResult AppointmentForm()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
