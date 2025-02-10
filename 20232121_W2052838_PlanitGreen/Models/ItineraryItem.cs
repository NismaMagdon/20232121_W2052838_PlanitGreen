using Microsoft.Extensions.Primitives;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class ItineraryItem
    {
        private int ItineraryItemID { get; set; }
        private Tour Tour { get; set; }
        private int Day { get; set; }
        private string Description { get; set; }
        private String Location { get; set; }

    }
}
