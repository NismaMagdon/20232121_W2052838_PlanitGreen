using System.ComponentModel.DataAnnotations;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class TourImage
    {
        [Key]
        public int ImageID { get; set; }
        public virtual Tour Tour { get; set; }
        public string Path { get; set; }
    }
}
