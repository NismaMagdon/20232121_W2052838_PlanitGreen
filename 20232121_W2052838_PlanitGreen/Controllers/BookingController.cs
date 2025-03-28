using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Booking page (User must be logged in)
        public IActionResult Book(int departureId)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null) {

                // Store the intended booking URL in session
                HttpContext.Session.SetString("ReturnUrl", Url.Action("Book", "Booking", new { departureId }, protocol: Request.Scheme));

                // Redirect to login page if not logged in
                return RedirectToAction("Login", "Account");
            }

            var departure = _context.Departure
            .Include(d => d.Tour) // Ensuring Tour data is included
            .FirstOrDefault(d => d.DepartureID == departureId);
            if (departure == null)
            {
                return NotFound();
            }

            var ecoPoints = _context.EcoPoints.FirstOrDefault(e => e.User.UserID == userId);
            ViewBag.AvailablePoints = ecoPoints?.AvailablePoints ?? 0; // Default to 0 if no points found


            return View(departure);

        }
    }
}
