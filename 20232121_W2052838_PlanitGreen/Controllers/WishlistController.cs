using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult MyWishlist()
        {

            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                // Store the intended dashboard URL in session
                HttpContext.Session.SetString("ReturnUrl", Url.Action("MyWishlist", "Wishlist"));

                // Redirect to login page if not logged in
                return RedirectToAction("Login", "Account");
            }

            var wishlistTours = _context.WishlistItem
                .Where(w => w.User.UserID == userId.Value)
                .Include(w => w.Tour)
                    .ThenInclude(t => t.TourStyle)
                .Include(w => w.Tour)
                    .ThenInclude(t => t.ImageList)
                .Select(w => w.Tour)
                .ToList();


            return View(wishlistTours);
        }

        // Action to add a tour to the wishlist
        [HttpPost]
        public IActionResult AddToWishlist(int tourId)
        {
            // Retrieve UserID from session
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId.HasValue)
            {
                var user = _context.User.FirstOrDefault(u => u.UserID == userId.Value);
                var tour = _context.Tour.FirstOrDefault(t => t.TourID == tourId);

                if (tour != null && user != null)
                {
                    // Check if this tour is already in the user's wishlist
                    var existingWishlistItem = _context.WishlistItem
                        .FirstOrDefault(w => w.User.UserID == user.UserID && w.Tour.TourID == tour.TourID);

                    // If the item isn't in the wishlist, add it
                    if (existingWishlistItem == null)
                    {
                        var wishlistItem = new WishlistItem
                        {
                            User = user,
                            Tour = tour
                        };
                        _context.WishlistItem.Add(wishlistItem);
                        _context.SaveChanges();
                    }
                }

                string referer = Request.Headers["Referer"].ToString();
                return Redirect(referer); // Redirects to previous page
            }
            else
            {
                // Store the current URL to return to after login
                HttpContext.Session.SetString("ReturnUrl", Request.Headers["Referer"].ToString());
                return RedirectToAction("Login", "Account"); // Redirect to login if no user ID in session
            }
        }


        // Action to remove a tour from the wishlist
        [HttpPost]
        public IActionResult RemoveFromWishlist(int tourId)
        {
            // Retrieve UserID from session
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId.HasValue)
            {
                var user = _context.User.FirstOrDefault(u => u.UserID == userId.Value);
                var tour = _context.Tour
                    .FirstOrDefault(t => t.TourID == tourId);

                if (tour != null && user != null)
                {
                    // Find the wishlist item to remove
                    var wishlistItem = _context.WishlistItem
                        .FirstOrDefault(w => w.User.UserID == user.UserID && w.Tour.TourID == tour.TourID);

                    if (wishlistItem != null)
                    {
                        _context.WishlistItem.Remove(wishlistItem);
                        _context.SaveChanges();
                    }
                }

                string referer = Request.Headers["Referer"].ToString();
                return Redirect(referer); // Redirects to previous page
            }
            else
            {
                // Store the current URL to return to after login
                HttpContext.Session.SetString("ReturnUrl", Request.Headers["Referer"].ToString());
                return RedirectToAction("Login", "Account"); // Redirect to login if no user ID in session
            }
        }

    }
}
