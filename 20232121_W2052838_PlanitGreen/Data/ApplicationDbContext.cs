using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace _20232121_W2052838_PlanitGreen.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Badge> Badge { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Departure> Departure { get; set; }
        public DbSet<Destination> Destination { get; set; }
        public DbSet<Donation> Donation { get; set; }
        public DbSet<EcoPoints> EcoPoints { get; set; }
        public DbSet<ItineraryItem> ItineraryItem { get; set; }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourImage> TourImage { get; set; }
        public DbSet<TourStyle> TourStyle { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserBadge> UserBadge { get; set; }
        public DbSet<WishlistItem> WishlistItem { get; set; }
    }
}
