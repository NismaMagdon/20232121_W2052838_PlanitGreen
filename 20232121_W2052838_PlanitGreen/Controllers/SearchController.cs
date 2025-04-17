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
            var tours = searchManager.GetToursByKeyword(searchQuery)
                ?.Where(t => t.IsActive)
                .ToList() ?? new List<Tour>();

           
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
            ViewData["DestinationId"] = null;
            ViewData["TourStyleId"] = null;
            return View(tours);
        }


        public IActionResult ByDestination(int id)
        {
            var tours = searchManager.GetToursByDestinationId(id)
                ?.Where(t => t.IsActive) // Only include active tours
                .ToList() ?? new List<Tour>();


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
            ViewData["DestinationId"] = id;
            ViewData["TourStyleId"] = null;
            return View("SearchResults", tours);
        }



        public IActionResult ByTourStyle(int id)
        {
            var tours = searchManager.GetToursByTourStyleId(id)
                ?.Where(t => t.IsActive)  // Only include active tours
                .ToList() ?? new List<Tour>();

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
            ViewData["TourStyleId"] = id;
            ViewData["DestinationId"] = null;
            return View("SearchResults", tours);
        }


        // Action to handle filtering and sorting (POST request)
        [HttpPost]
        public IActionResult FilterAndSort(string? searchQuery, int? destinationId, int? tourStyleId, double? minPrice, double? maxPrice, string? style, DateOnly? startDate, DateOnly? endDate, string? sortOrder)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                var tours = searchManager.GetToursByKeyword(searchQuery);  // Returns IQueryable<Tour>
                tours = searchManager.ApplyFilterAndSort(tours, sortOrder, minPrice, maxPrice, style, startDate, endDate);  // Apply filters/sorting
                ViewData["SearchQuery"] = searchQuery;
                ViewData["DestinationId"] = destinationId;
                ViewData["TourStyleId"] = tourStyleId;
                return View("SearchResults", tours.ToList());  // Convert to List<Tour> to pass to the view
            }
            else if (destinationId.HasValue)
            {
                var tours = searchManager.GetToursByDestinationId(destinationId.Value);
                tours = searchManager.ApplyFilterAndSort(tours, sortOrder, minPrice, maxPrice, style, startDate, endDate);  // Apply filters/sorting
                ViewData["SearchQuery"] = searchQuery;
                ViewData["DestinationId"] = destinationId;
                ViewData["TourStyleId"] = tourStyleId;
                return View("SearchResults", tours.ToList());  // Convert to List<Tour> to pass to the view
            }
            else if (tourStyleId.HasValue)
            {
                var tours = searchManager.GetToursByTourStyleId(tourStyleId.Value);
                tours = searchManager.ApplyFilterAndSort(tours, sortOrder, minPrice, maxPrice, style, startDate, endDate);  // Apply filters/sorting
                ViewData["SearchQuery"] = searchQuery;
                ViewData["DestinationId"] = destinationId;
                ViewData["TourStyleId"] = tourStyleId;
                return View("SearchResults", tours.ToList());  // Convert to List<Tour> to pass to the view
            }
            else
            {
                var tours = searchManager.GetToursByKeyword(searchQuery);
                tours = searchManager.ApplyFilterAndSort(tours, sortOrder, minPrice, maxPrice, style, startDate, endDate);  // Apply filters/sorting
                ViewData["SearchQuery"] = searchQuery;
                ViewData["DestinationId"] = destinationId;
                ViewData["TourStyleId"] = tourStyleId;
                return View("SearchResults", tours.ToList());  // Convert to List<Tour> to pass to the view
            }

            

            
        }
    }
}
