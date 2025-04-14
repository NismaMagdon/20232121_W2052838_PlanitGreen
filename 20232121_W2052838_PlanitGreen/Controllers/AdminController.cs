using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Filters;
using _20232121_W2052838_PlanitGreen.Managers;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult ManageDepartures()
        {
            return View();
        }

        public IActionResult ManageBookings()
        {
            return View();
        }

        public IActionResult ManageAccounts()
        {
            return View();
        }
    }
}
