using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20232121_W2052838_PlanitGreen.Models

{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public virtual User User { get; set; }
        public virtual Departure Departure { get; set; }
        public int PassengerQty { get; set; }
        public bool IsPublicTransport { get; set; }
        public int EcoPointsUsed { get; set; }
        public double TotalPrice { get; set; }
        
        public List<Passenger> PassengerList { get; set; } = new List<Passenger>();
    }
}
