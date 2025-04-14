using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Filters;
using _20232121_W2052838_PlanitGreen.Managers;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    [RejectIfTraveller]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly SearchManager searchManager;

        public AdminController(ApplicationDbContext context, SearchManager searchManager)
        {
            _context = context;
            this.searchManager = searchManager;

        }

        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.User.FirstOrDefault(u => u.UserID == userId);

            if (user == null || user.Role != Role.Admin)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(user);
        }

        public IActionResult ManageTours(string? searchQuery)
        {
            var tours = searchManager.GetToursByKeyword(searchQuery)?.ToList() ?? new List<Tour>();
            return View(tours);
        }

        public IActionResult ManageTourDetails(int id)
        {
            var tour = _context.Tour
                .Include(t => t.TourStyle)
                .Include(t => t.Destination)
                .Include(t => t.DepartureList)
                .FirstOrDefault(t => t.TourID == id);

            if (tour == null)
                return NotFound();

            return View(tour);
        }

        [HttpPost]
        public IActionResult ToggleTourStatus(int tourId)
        {
            var tour = _context.Tour.Find(tourId);
            if (tour == null) return NotFound();

            tour.IsActive = !tour.IsActive;
            _context.SaveChanges();

            return RedirectToAction("ManageTourDetails", new { id = tourId });
        }

        [HttpPost]
        public IActionResult AddDeparture(int tourId, DateOnly StartDate, DateOnly EndDate, int PacksLimit)
        {
            var tour = _context.Tour.Include(t => t.DepartureList).FirstOrDefault(t => t.TourID == tourId);
            if (tour == null) return NotFound();

            var newDeparture = new Departure
            {
                Tour = tour,
                StartDate = StartDate,
                EndDate = EndDate,
                PacksLimit = PacksLimit,
                PacksQty = 0,
                Iscancelled = false
            };

            tour.DepartureList.Add(newDeparture);
            _context.SaveChanges();

            return RedirectToAction("ManageTourDetails", new { id = tourId });
        }

        public IActionResult ViewDepartureBookings(int departureId)
        {
            var bookings = _context.Booking
                .Where(b => b.Departure.DepartureID == departureId)
                .Include(b => b.User)
                .Include(b => b.PassengerList)
                .Include(b => b.Departure)
                .ToList();

            ViewData["DepartureId"] = departureId;

            return View("DepartureBookings",bookings);
        }

        public IActionResult ImpactDashboard()
        {
            var totalTrees = _context.User.Sum(u => u.TreesPlanted);
            var totalPublicTransportBookings = _context.Booking.Count(b => b.IsPublicTransport);
            var totalEcoPointsDonated = _context.Donation.Sum(d => d.Amount);

            ViewBag.TotalTrees = totalTrees;
            ViewBag.PublicTransportBookings = totalPublicTransportBookings;
            ViewBag.TotalEcoPoints = totalEcoPointsDonated;

            return View();
        
        }

        public IActionResult ManageAccounts()
        {
            return View();
        }
    }
}
