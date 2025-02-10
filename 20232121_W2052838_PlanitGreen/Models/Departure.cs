namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Departure
    {
        private int DepartureID { get; set; }
        private Tour Tour { get; set; }
        private DateOnly StartDate { get; set; }
        private DateOnly EndDate { get; set; }
        private int PacksLimit { get; set; }
        private int PacksQty { get; set; }
        private bool Iscancelled { get; set; }
        private List<Booking> BookingList { get; set; }


    }
}
