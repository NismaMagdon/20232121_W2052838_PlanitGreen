namespace _20232121_W2052838_PlanitGreen.Models
{
    public class DashboardViewModel
    {
        public string FirstName { get; set; }

        public int AvailableEcoPoints { get; set; }
        public int TotalEcoPoints { get; set; }

        public int TreesPlanted { get; set; }

        public List<Badge> EarnedBadges { get; set; } = new();
        public List<Badge> PendingBadges { get; set; } = new();

        public int TotalBadgesEarned => EarnedBadges?.Count ?? 0;

        public List<UserLeaderboardEntry> Leaderboard { get; set; } = new();
        public int DonationAmount { get; set; }
    }
}
