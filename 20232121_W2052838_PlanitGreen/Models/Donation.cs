namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Donation
    {
        public int DonationID { get; set; }
        public virtual User User { get; set; }
        public int Amount { get; set; }
    }
}
