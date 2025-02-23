using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;

namespace _20232121_W2052838_PlanitGreen.Managers
{
    public class UserManager
    {
        private readonly ApplicationDbContext _context;

        public UserManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public void RegisterUser(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();

            // Now create a related EcoPoints instance with default 0 points
            var ecoPoints = new EcoPoints
            {
                User = user,  
                TotalPoints = 0,      
                AvailablePoints = 0    
            };

            // Add EcoPoints to the database
            _context.EcoPoints.Add(ecoPoints);
            _context.SaveChanges();  // Save EcoPoints to the database
        }
    }
}
