using _20232121_W2052838_PlanitGreen.Data;
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
    }
}
