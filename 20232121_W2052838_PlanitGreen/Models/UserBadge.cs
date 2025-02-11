using System.ComponentModel.DataAnnotations;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class UserBadge
    {
        [Key]
        public int UserBadgeID { get; set; }
        public virtual Badge Badge { get; set; }
        public virtual User User { get; set; }
    }
}
