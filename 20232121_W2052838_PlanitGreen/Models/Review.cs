namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Review
    {
        private int ReviewID { get; set; }
        private User User { get; set; }
        private Tour Tour { get; set; }
        private string Content { get; set; }
        private DateTime CreatedAt { get; set; }
    }
}
