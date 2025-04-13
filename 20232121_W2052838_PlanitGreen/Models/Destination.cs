using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Destination
    {
        [Key]
        public int DestinationID { get; set; }
        public string DestinationName { get; set; }

   
        public List<Tour> TourList { get; set; } = new List<Tour>();
    }
}
