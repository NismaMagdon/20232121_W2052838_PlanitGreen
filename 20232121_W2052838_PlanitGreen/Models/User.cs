using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.Traveller;
        public int TreesPlanted { get; set; } = 0;

    

        
        public List<WishlistItem> Wishlist { get; set; } = new List<WishlistItem>();
        
        public List<UserBadge> UserBadge { get; set; } = new List<UserBadge>();

    }
}
