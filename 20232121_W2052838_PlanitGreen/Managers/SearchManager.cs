using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Managers
{
    public class SearchManager
    {
        private readonly ApplicationDbContext _context;


        public SearchManager(ApplicationDbContext context)
        {
            _context = context;
        }



        // Method to get tours based on the search query
        public IQueryable<Tour> GetToursByKeyword(string searchQuery)
        {
            // Eager load related entities like TourStyle and Destination
            var tours = _context.Tour
                                .Include(t => t.TourStyle)   // Eager load the TourStyle
                                .Include(t => t.Destination) // Eager load the Destination
                            
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                tours = tours.Where(tour => tour.TourName.Contains(searchQuery) ||
                                             tour.TourStyle.TourStyleName.Contains(searchQuery) ||
                                             tour.Destination.DestinationName.Contains(searchQuery));
            }

            var tourImages = _context.TourImage
                             .Where(img => img.Tour != null)  // Ensure we only get images associated with tours
                             .ToList();

            // Associate the first image with each tour (or any logic you need for selecting images)
            foreach (var tour in tours)
            {
                // Get the first image for this tour (or adjust logic as needed)
                var firstImage = tourImages.FirstOrDefault(img => img.Tour.TourID == tour.TourID);
                if (firstImage != null)
                {
                    // You can directly add the image path to a property in your view model if needed
                    tour.ImageList = new List<TourImage> { firstImage };  // For demonstration, or just set the path
                }
            }

            return tours;

        }



        // Method to apply filters and sorting to the tours
        public IQueryable<Tour> ApplyFilterAndSort(IQueryable<Tour> tours, string sortOrder, double? minPrice, double? maxPrice, string? style, DateOnly? startDate, DateOnly? endDate)
        {
            // Apply price range filter
            if (minPrice.HasValue)
            {
                tours = tours.Where(t => t.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                tours = tours.Where(t => t.Price <= maxPrice);
            }

            // Apply style filter
            if (!string.IsNullOrEmpty(style))
            {
                tours = tours.Where(t => t.TourStyle.TourStyleName == style);
            }

            // Apply date range filters
            if (startDate.HasValue)
            {
                tours = tours.Where(t => t.DepartureList.Any(d => d.StartDate >= startDate));
            }

            if (endDate.HasValue)
            {
                tours = tours.Where(t => t.DepartureList.Any(d => d.EndDate <= endDate));
            }

            // Sorting logic
            switch (sortOrder)
            {
                case "ecoPoints":
                    tours = tours.OrderByDescending(t => t.EcoPoints); // Eco Points: high to low
                    break;
                case "price-low":
                    tours = tours.OrderBy(t => t.Price); // Price: low to high
                    break;
                case "price-high":
                    tours = tours.OrderByDescending(t => t.Price); // Price: high to low
                    break;
                default:
                    tours = tours.OrderByDescending(t => t.EcoPoints); // Eco Points: high to low
                    break;
            }

            return tours;
        }
    }
}
