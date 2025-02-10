namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Booking
    {
        private int BookingID { get; set; }
        private User User { get; set; }
        private Departure Departure { get; set; }
        private int PassengerQty    { get; set; }
        private bool IsPublicTransport { get; set; }
        private int EcoPointsUsed { get; set; }
        private double TotalPrice { get; set; }
        private List<Passenger> PassengerList { get; set; }
    }
}
