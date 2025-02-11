using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class TourStyle
    {
        [Key]
        public int TourStyleID { get; set; }
        public string TourStyleName { get; set; }
        public string StyleDescription { get; set; }

        [NotMapped]
        public List<Tour> TourList { get; set; } = new List<Tour>();
    }
}
