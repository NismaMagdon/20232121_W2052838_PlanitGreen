namespace _20232121_W2052838_PlanitGreen.Models
{
    public class User
    {
        private int UserID { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private DateOnly Dob { get; set; }
        private string Email { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private Role Role { get; set; }
        private List<WishlistItem> Wishlist { get; set; }
        private List<UserBadge> UserBadge { get; set; }
    }
}
