using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using The_Glamour.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace The_Glamour.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IConfiguration _config;

        public AppointmentsController(IConfiguration config)
        {
            _config = config;
            Appointments.ConnectionString = _config.GetConnectionString("DefaultConnection");
        }

        // ----------- APPOINTMENT CRUD ----------
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LogIn", "Home");

            var appointments = Appointments.GetByUserId(userId.Value);
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromBody] Appointments appointment)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                    return Json(new { success = false, message = "Session expired. Please log in again." });

                appointment.UserId = userId.Value;

                // Validate required fields
                if (string.IsNullOrEmpty(appointment.Name))
                    ModelState.AddModelError("Name", "Name is required");

                if (string.IsNullOrEmpty(appointment.Service))
                    ModelState.AddModelError("Service", "Service is required");

                if (appointment.Date == default)
                    ModelState.AddModelError("Date", "Date is required");

                // Time validation
                if (string.IsNullOrEmpty(appointment.TimeString))
                {
                    ModelState.AddModelError("Time", "Time is required");
                }
                else
                {
                    if (!TimeSpan.TryParse(appointment.TimeString, out var time))
                    {
                        ModelState.AddModelError("Time", "Invalid time format. Please use HH:MM format.");
                    }
                    else
                    {
                        appointment.Time = time;
                    }
                }

                if (ModelState.IsValid)
                {
                    appointment.CreatedAt = DateTime.UtcNow; // Use UTC for database consistency
                    appointment.UpdatedAt = DateTime.UtcNow;

                    if (appointment.Create())
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Appointment booked successfully!",
                            appointmentId = appointment.AppointmentId
                        });
                    }
                    return Json(new
                    {
                        success = false,
                        message = "Failed to save appointment to database. Please try again."
                    });
                }

                // Collect validation errors
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new
                {
                    success = false,
                    message = "Validation errors occurred.",
                    errors
                });
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Error creating appointment: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    error = ex.Message
                });
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LogIn", "Home");

            var appointment = Appointments.GetById(id);
            if (appointment == null || appointment.UserId != userId.Value)
            {
                return NotFound();
            }
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Appointments appointment)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null) return RedirectToAction("LogIn", "Home");

                if (id != appointment.AppointmentId) return NotFound();

                if (ModelState.IsValid && appointment.Update())
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Failed to update appointment";
                return View(appointment);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error: {ex.Message}";
                return View(appointment);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LogIn", "Home");

            var appointment = Appointments.GetById(id);
            if (appointment == null || appointment.UserId != userId.Value)
            {
                return NotFound();
            }
            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null) return RedirectToAction("LogIn", "Home");

                if (Appointments.Delete(id))
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "Failed to delete appointment";
                return View(Appointments.GetById(id));
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error: {ex.Message}";
                return View(Appointments.GetById(id));
            }
        }

        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LogIn", "Home");

            var appointment = Appointments.GetById(id);
            if (appointment == null || appointment.UserId != userId.Value)
            {
                return NotFound();
            }
            return View(appointment);
        }
    }
}