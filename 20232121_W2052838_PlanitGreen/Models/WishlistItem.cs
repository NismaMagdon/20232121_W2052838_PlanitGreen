using System.ComponentModel.DataAnnotations;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class WishlistItem
    {
        [Key]
        public int WishlistItemID { get; set; }
        public virtual User User { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
