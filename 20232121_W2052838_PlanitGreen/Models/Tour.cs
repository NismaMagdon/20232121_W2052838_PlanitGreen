using _20232121_W2052838_PlanitGreen.Managers;
using Microsoft.EntityFrameworkCore;
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
            const double benchmarkFootprint = 100.0; // A reference value for comparison

            double carbonSavings = benchmarkFootprint - this.CarbonFootprint;

            if (carbonSavings > 0)
            {
                baseEcoPoints += (int)carbonSavings;
            }

            //Factor 2: Tour duration - Longer tours are rewarded
            int extraDays = Duration - 7;
            if (extraDays > 0)
            {
                baseEcoPoints += extraDays * 2;
            }

            //Factor 3: Tree planting incentives
            baseEcoPoints += this.TreesPlanted * 5; //5 points per tree planted

            //Factor 4: Tour style adjustment
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
