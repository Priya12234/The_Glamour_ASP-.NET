using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using The_Glamour.Models;
using System.Diagnostics;

namespace The_Glamour.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
            Users.ConnectionString = _config.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AppointmentForm()
        {
            return View();
        }

        // ----------- LOGIN ----------
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(string email, string password)
        {
            try
            {
                var user = Users.Login(email, password);
                if (user != null)
                {
                    // Store user information in session
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", user.Name);

                    // Set session timeout (optional)
                    HttpContext.Session.SetInt32("SessionTimeout", (int)TimeSpan.FromMinutes(30).TotalSeconds);

                    return RedirectToAction("Index");
                }

                ViewBag.Error = "Invalid email or password";
                return View();
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Login error: {ex.Message}");
                ViewBag.Error = "An error occurred during login. Please try again.";
                return View();
            }
        }

        // ----------- REGISTER ----------
        [HttpGet]
        public IActionResult RegisterForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterForm(Users user)
        {
            bool success = user.Register();
            if (success)
            {
                ViewBag.Message = "Registration Successful";
                return RedirectToAction("LogIn");
            }

            ViewBag.Error = "Registration Failed";
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult OurServices()
        {
            return View();
        }

        // Example of protected action using session
        public IActionResult MyProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("LogIn");
            }

            var user = Users.GetById(userId.Value);
            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("LogIn");
            }

            return View(user);
        }
    }
}
