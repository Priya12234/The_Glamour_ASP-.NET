
using Microsoft.AspNetCore.Mvc;
using The_Glamour.Models;
using System.Collections.Generic;
using System.Linq;

namespace The_Glamour.Controllers
{
    public class AdminController : Controller
    {
        private static List<User> users = new List<User>();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Appointments()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View(users);
        }

        [HttpGet]
        public IActionResult RegistrationForm()
        {
            return View("RegistrationForm", new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrationForm(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("RegistrationForm", user);
            }

            // Your registration logic here
            return RedirectToAction("Users");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var userToDelete = users.FirstOrDefault(u => u.Id == id);
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                return Ok();
            }
            return NotFound();
        }
    }
}