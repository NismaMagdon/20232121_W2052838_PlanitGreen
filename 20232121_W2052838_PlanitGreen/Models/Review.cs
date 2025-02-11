using System.ComponentModel.DataAnnotations;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        public virtual User User { get; set; }
        public virtual Tour Tour { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
