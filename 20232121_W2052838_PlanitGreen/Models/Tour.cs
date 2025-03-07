using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Tour
    {
        [Key]
        public int TourID { get; set; }
        public string TourName { get; set; }
        public string Description { get; set; }
        public virtual TourStyle TourStyle { get; set; }
        public int EcoPoints { get; set; }
        public virtual Destination Destination { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public string CarbonFootprint { get; set; }
        public int TreesPlanted { get; set; }
        public bool IsActive { get; set; }

        
        public List<ItineraryItem> Itinerary { get; set; } = new List<ItineraryItem>();
        
        public List<TourImage> ImageList { get; set; } = new List<TourImage>();
        
        public List<Review> Reviews { get; set; } = new List<Review>();
        
        public List<Departure> DepartureList { get; set; } = new List<Departure>();


    }
}
