using System.ComponentModel.DataAnnotations;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Passenger
    {
        [Key]
        public int PassengerID { get; set; }
        public virtual Booking Booking { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MealType  { get; set; }

    }
}
