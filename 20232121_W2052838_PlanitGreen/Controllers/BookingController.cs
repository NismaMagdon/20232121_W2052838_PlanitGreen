using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Managers;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BadgeEvaluator _badgeEvaluator;

        public BookingController(ApplicationDbContext context, BadgeEvaluator badgeEvaluator)
        {
            _context = context;
            _badgeEvaluator = badgeEvaluator;
        }

        //GET: Booking page (User must be logged in)
        public IActionResult Book(int departureId)
        {
            // Check if user is logged in by looking for UserID in the session
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null) {

                // Store the intended booking URL in session
                HttpContext.Session.SetString("ReturnUrl", Url.Action("Book", "Booking", new { departureId }, protocol: Request.Scheme));

                // Redirect to login page if not logged in
                return RedirectToAction("Login", "Account");
            }

            var departure = _context.Departure
            .Include(d => d.Tour) // Ensuring Tour data is included
            .ThenInclude(t => t.TourStyle)
            .FirstOrDefault(d => d.DepartureID == departureId);
            if (departure == null)
            {
                return NotFound();
            }

            var ecoPoints = _context.EcoPoints.FirstOrDefault(e => e.User.UserID == userId);
            ViewBag.AvailablePoints = ecoPoints?.AvailablePoints ?? 0; // Default to 0 if no points found
            ViewBag.TourStyleName = departure.Tour?.TourStyle?.TourStyleName;

            return View(departure);

        }

        [HttpPost]
        public IActionResult ConfirmBooking(int DepartureID, int passengerCount, int redeemPoints, bool IsPublicTransport, List<Passenger> Passengers)
        {
            var departure = _context.Departure
                .Include (d => d.Tour)
                .Include(d => d.Tour.TourStyle)
               .FirstOrDefault(d => d.DepartureID == DepartureID);

            var userId = HttpContext.Session.GetInt32("UserID");
            var user = _context.User.FirstOrDefault(u => u.UserID == userId);

            // Create an instance of BookingManager
            var bookingManager = new BookingManager(_context, _badgeEvaluator);

            var bookingResult = bookingManager.CreateBooking(
                departure,
                user,
                passengerCount,
                redeemPoints,
                IsPublicTransport,
                Passengers
            );

            if (bookingResult == null)
            {
                return BadRequest("Booking could not be created.");
            }

            return RedirectToAction("BookingConfirmation", new { id = bookingResult.BookingID });
        }

        //GET: Booking confirmation
        public IActionResult BookingConfirmation(int id)
        {
            var booking = _context.Booking
                .Include(b => b.Departure)
                    .ThenInclude(d => d.Tour)
                    .ThenInclude(t => t.TourStyle)
                .Include(b => b.User)
                .FirstOrDefault(b => b.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}
