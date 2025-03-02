using _20232121_W2052838_PlanitGreen.Managers;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchManager searchManager;

        public SearchController(SearchManager searchManager)
        {
            this.searchManager = searchManager;
        }

        // Action to handle the initial search (GET request)
        public IActionResult SearchResults(string searchQuery)
        {
            var tours = searchManager.GetToursByKeyword(searchQuery)?.ToList() ?? new List<Tour>();

            ViewData["SearchQuery"] = searchQuery;  // Keep search query in the view for input persistence
            return View(tours);
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
