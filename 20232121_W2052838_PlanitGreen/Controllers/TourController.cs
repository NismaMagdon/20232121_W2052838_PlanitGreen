using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TourController(ApplicationDbContext context)
        {
            _context= context;
        }
        public IActionResult Details(int id)
        {
            var tour = _context.Tour
                .Include(t => t.TourStyle)
                .Include(t => t.Itinerary)
                .Include(t => t.ImageList)
                .Include(t => t.Reviews)
                .ThenInclude(r => r.User)
                .Include(t => t.DepartureList)
                .FirstOrDefault(t => t.TourID == id);

            if (tour == null)
            {
                return NotFound();
            }

            // Get the wishlist IDs if user is logged in
            int? userId = HttpContext.Session.GetInt32("UserID");
            List<int> wishlistTourIds = new();

            if (userId.HasValue)
            {
                wishlistTourIds = _context.WishlistItem
                    .Where(w => w.User.UserID == userId.Value)
                    .Select(w => w.Tour.TourID)
                    .ToList();
            }

            ViewBag.WishlistTourIds = wishlistTourIds;
            return View(tour);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(int tourId, string content)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {

                HttpContext.Session.SetString("ReturnUrl", Url.Action("Details", "Tour", new { id = tourId }, protocol: Request.Scheme));

                // Redirect to login page if not logged in
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ReviewError"] = "Review cannot be empty.";
                return RedirectToAction("Details", new { id = tourId });
            }

            var tour = await _context.Tour.FindAsync(tourId);
            var user = await _context.User.FindAsync(userId.Value);

            if (tour == null || user == null)
            {
                return NotFound();
            }

            var review = new Review
            {
                Tour = tour,
                User = user,
                Content = content.Trim(),
                CreatedAt = DateTime.Now
            };

            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = tourId });
        }
    }
}
