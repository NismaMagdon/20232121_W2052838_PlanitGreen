using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;

namespace _20232121_W2052838_PlanitGreen.Managers
{
    public class DonationManager
    {
        private readonly ApplicationDbContext _context;
        private readonly BadgeEvaluator _badgeEvaluator;

        public DonationManager(ApplicationDbContext context, BadgeEvaluator badgeEvaluator)
        {
            _context = context;
            _badgeEvaluator = badgeEvaluator;
        }

        public Donation DonateEcoPoints(User user, int pointsToDonate)
        {
            if (user == null || pointsToDonate <= 0) return null;

            var ecoPoints = _context.EcoPoints.FirstOrDefault(e => e.User.UserID == user.UserID);
            if (ecoPoints == null || ecoPoints.AvailablePoints < pointsToDonate)
                return null;

            // Deduct points
            ecoPoints.AvailablePoints -= pointsToDonate;
            _context.EcoPoints.Update(ecoPoints);

            // Create donation record
            var donation = new Donation
            {
                User = user,
                Amount = pointsToDonate
            };
            _context.Donation.Add(donation);

            // Update trees planted (1 tree per 10 points)
            int trees = pointsToDonate / 10;
            user.TreesPlanted += trees;
            _context.User.Update(user);

            // Save all changes
            _context.SaveChanges();

            // Evaluate badges
            _badgeEvaluator.EvaluateBadgesAsync(user).Wait();

            return donation;
        }
    }

   
        
            

}