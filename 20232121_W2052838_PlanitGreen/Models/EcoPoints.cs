using System.ComponentModel.DataAnnotations;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class EcoPoints
    {
        [Key]
        public int PointsID {  get; set; }
        public virtual User User { get; set; }
        public int TotalPoints { get; set; }
        public int AvailablePoints { get; set; }


    }
}
