using _20232121_W2052838_PlanitGreen.Managers;
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
        public virtual Destination Destination { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public double CarbonFootprint { get; set; }
        public int TreesPlanted { get; set; }
        public bool IsActive { get; set; }

        
        public List<ItineraryItem> Itinerary { get; set; } = new List<ItineraryItem>();
        
        public List<TourImage> ImageList { get; set; } = new List<TourImage>();
        
        public List<Review> Reviews { get; set; } = new List<Review>();
        
        public List<Departure> DepartureList { get; set; } = new List<Departure>();


        public int CalculateEcoPoints()
        {
            int baseEcoPoints = 100; // Every tour gets at least 100 points

            // Factor 1: Carbon Footprint Reduction
            double averageCarbonFootprint = 100.0; // Assume an avg carbon footprint of 50kg

            if ((averageCarbonFootprint - this.CarbonFootprint) > 0)
            {
                baseEcoPoints += (int)(averageCarbonFootprint - this.CarbonFootprint);
            }

            //Factor 2: Tour duration
            if (this.Duration > 7) // If the trip duration is more than 7 days, add points
            {
                baseEcoPoints += (this.Duration - 7) * 2;
            }

            //Factor 3: Tree planting
            baseEcoPoints += this.TreesPlanted * 5; //5 points per tree planted

            //Factor 4: Tour style
            switch(this.TourStyle.TourStyleName)
            {

                case "Leisure":
                case "Adventure":
                    baseEcoPoints -= 10;
                    break;
            }

            return baseEcoPoints;
        }


    }
}
