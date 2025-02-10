namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Tour
    {
        private int TourID { get; set; }
        private string TourName { get; set; }
        private string Description { get; set; }
        private TourStyle TourStyle { get; set; }
        private int EcoPoints { get; set; }
        private Destination Destination { get; set; }
        private int Duration { get; set; }
        private double Price { get; set; }
        private string CarbonFootprint { get; set; }
        private int TreesPlanted { get; set; }
        private bool IsActive { get; set; }
        private List<ItineraryItem> Itinerary { get; set; }
        private List<TourImage> ImageList { get; set; }
        private List<Review> Reviews { get; set; }
        private List<Departure> DepartureList { get; set; }


    }
}
