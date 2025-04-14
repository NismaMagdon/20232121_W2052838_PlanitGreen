using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Filters;
using _20232121_W2052838_PlanitGreen.Managers;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    [RejectIfAdmin]
    public class SearchController : Controller
    {
        private readonly SearchManager searchManager;
        private readonly ApplicationDbContext _context;

        public SearchController(SearchManager searchManager, ApplicationDbContext context)
        {
            this.searchManager = searchManager;
            _context = context;
        }

        // Action to handle the initial search (GET request)
        public IActionResult SearchResults(string searchQuery)
        {
            var tours = searchManager.GetToursByKeyword(searchQuery)?.ToList() ?? new List<Tour>();

           
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
            ViewData["SearchQuery"] = searchQuery;  // Keep search query in the view for input persistence
            return View(tours);
        }


        public IActionResult ByDestination(int id)
        {
            var tours = searchManager.GetToursByDestinationId(id);

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
            ViewData["SearchQuery"] = null; 
            return View("SearchResults", tours);
        }



        public IActionResult ByTourStyle(int id)
        {
            var tours = searchManager.GetToursByTourStyleId(id);

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
            ViewData["SearchQuery"] = null;
            return View("SearchResults", tours);
        }


        // Action to handle filtering and sorting (POST request)
        [HttpPost]
        public IActionResult FilterAndSort(string searchQuery, double? minPrice, double? maxPrice, string? style, DateOnly? startDate, DateOnly? endDate, string? sortOrder)
        {
            var tours = searchManager.GetToursByKeyword(searchQuery);  // Returns IQueryable<Tour>
            tours = searchManager.ApplyFilterAndSort(tours, sortOrder, minPrice, maxPrice, style, startDate, endDate);  // Apply filters/sorting
            ViewData["SearchQuery"] = searchQuery;
            return View("SearchResults", tours.ToList());  // Convert to List<Tour> to pass to the view
        }
    }
}
