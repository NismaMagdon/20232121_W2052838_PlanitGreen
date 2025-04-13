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
                                .Include(t => t.TourStyle)   
                                .Include(t => t.Destination)
                                .Include(t => t.DepartureList)

                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                tours = tours.Where(tour => tour.TourName.Contains(searchQuery) ||
                                            tour.Description.Contains(searchQuery) ||
                                             tour.TourStyle.TourStyleName.Contains(searchQuery) ||
                                             tour.Destination.DestinationName.Contains(searchQuery));
            }

            var tourImages = _context.TourImage
                             .Where(img => img.Tour != null)  // Ensure we only get images associated with tours
                             .ToList();

            // Associate the first image with each tour (or any logic you need for selecting images)
            foreach (var tour in tours)
            {
                // Get the first image for this tour
                var firstImage = tourImages.FirstOrDefault(img => img.Tour != null && img.Tour.TourID == tour.TourID);
                if (firstImage != null)
                {
                    // You can directly add the image path to a property in your view model if needed
                    tour.ImageList = new List<TourImage> { firstImage };  // For demonstration, or just set the path
                }
            }

            return tours;

        }

        public List<Tour> GetToursByDestinationId(int destinationId)
        {
            // Eager load related entities like TourStyle, Destination, and ImageList
            var tours = _context.Tour
                                .Include(t => t.TourStyle)  
                                .Include(t => t.Destination)
                                .Include(t => t.DepartureList)
                                .Include(t => t.ImageList)  
                                .Where(t => t.Destination.DestinationID == destinationId)
                                .ToList();

            // Associate the first image with each tour if needed (like in GetToursByKeyword)
            var tourImages = _context.TourImage
                                     .Where(img => img.Tour != null)
                                     .ToList();

            foreach (var tour in tours)
            {
                // Get the first image for this tour
                var firstImage = tourImages.FirstOrDefault(img => img.Tour != null && img.Tour.TourID == tour.TourID);
                if (firstImage != null)
                {
                    // Assign the first image to the tour's ImageList if you need it
                    tour.ImageList = new List<TourImage> { firstImage };
                }
            }

            return tours;
        }


        public List<Tour> GetToursByTourStyleId(int tourStyleId)
        {
            // Eager load related entities like TourStyle, Destination, and ImageList
            var tours = _context.Tour
                                .Include(t => t.TourStyle)   
                                .Include(t => t.Destination)
                                .Include(t => t.DepartureList)
                                .Include(t => t.ImageList)   
                                .Where(t => t.TourStyle.TourStyleID == tourStyleId)
                                .ToList();

            // Associate the first image with each tour if needed (like in GetToursByKeyword)
            var tourImages = _context.TourImage
                                     .Where(img => img.Tour != null)
                                     .ToList();

            foreach (var tour in tours)
            {
                // Get the first image for this tour
                var firstImage = tourImages.FirstOrDefault(img => img.Tour != null && img.Tour.TourID == tour.TourID);
                if (firstImage != null)
                {
                    // Assign the first image to the tour's ImageList if you need it
                    tour.ImageList = new List<TourImage> { firstImage };
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
                tours = tours.Where(t => t.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                tours = tours.Where(t => t.Price <= maxPrice.Value);
            }

            // Apply style filter
            if (!string.IsNullOrEmpty(style))
            {
                tours = tours.Where(t => t.TourStyle != null && t.TourStyle.TourStyleName == style);

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
                    tours = tours.AsEnumerable()  // Force in-memory evaluation
                                 .OrderByDescending(t => t.CalculateEcoPoints())
                                 .AsQueryable();
                    break;
                case "price-low":
                    tours = tours.OrderBy(t => t.Price);
                    break;
                case "price-high":
                    tours = tours.OrderByDescending(t => t.Price);
                    break;
                default:
                    tours = tours.AsEnumerable()
                                 .OrderByDescending(t => t.CalculateEcoPoints())
                                 .AsQueryable();
                    break;
            }

            return tours;
        }
    }
}
