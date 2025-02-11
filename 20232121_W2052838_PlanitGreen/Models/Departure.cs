using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Departure
    {
        [Key]
        public int DepartureID { get; set; }
        public virtual Tour Tour { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int PacksLimit { get; set; }
        public int PacksQty { get; set; }
        public bool Iscancelled { get; set; }

        [NotMapped]
        public List<Booking> BookingList { get; set; } = new List<Booking>();


    }
}
