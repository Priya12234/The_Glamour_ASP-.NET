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

        public IActionResult AppointmentForm()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult RegisterForm()
        {
            return View();
        }

        public IActionResult OurServices()
        {
            return View();
        }


        public IActionResult MyProfile()
        {
            return View();
        }
        public IActionResult Mycart()
        {
            return View();
        }
        public IActionResult Myappoientment()
        {
            return View();
        }
        public IActionResult editprofile()
        {
            return View();
        }
    }
}
