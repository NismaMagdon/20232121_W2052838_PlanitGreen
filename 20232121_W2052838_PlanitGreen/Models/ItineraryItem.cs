using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class ItineraryItem
    {
        [Key]
        public int ItineraryItemID { get; set; }
        public virtual Tour Tour { get; set; }
        public int Day { get; set; }
        public string Description { get; set; }
        public String Location { get; set; }

    }
}
