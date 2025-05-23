﻿using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Managers
{
    public class BadgeEvaluator
    {

        private readonly ApplicationDbContext _context;

        public BadgeEvaluator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task EvaluateBadgesAsync(User user)
        {
            var userId = user.UserID;

            // Gather counts
            int bookingCount = await _context.Booking.CountAsync(b => b.User.UserID == userId);
            int publicTransportCount = await _context.Booking.CountAsync(b => b.User.UserID == userId && b.IsPublicTransport);
            int donationCount = await _context.Donation.CountAsync(d => d.User.UserID == userId);
            int treesPlanted = user.TreesPlanted;

            // Build a dictionary to map criteria to actual values
            var userProgress = new Dictionary<string, int>
            {
                { "BookingCount", bookingCount },
                { "PublicTransportCount", publicTransportCount },
                { "DonationsCount", donationCount },
                { "TreesPlanted", treesPlanted }
            };

            // Loop over each criteria type
            foreach (var entry in userProgress)
            {
                string criteriaType = entry.Key;
                int userValue = entry.Value;

                // Get all badges for this criteria
                var allBadges = await _context.Badge
                    .Where(b => b.CriteriaType == criteriaType)
                    .ToListAsync();

                foreach (var badge in allBadges)
                {
                    bool qualifies = userValue >= badge.ThresholdValue;
                    var userBadge = await _context.UserBadge
                        .FirstOrDefaultAsync(ub => ub.User.UserID == userId && ub.Badge.BadgeID == badge.BadgeID);

                    if (qualifies && userBadge == null)
                    {
                        // Grant badge
                        _context.UserBadge.Add(new UserBadge
                        {
                            User = user,
                            Badge = badge
                        });

                        // Add eco points
                        if (badge.BonusEcoPoints > 0)
                        {
                            var ecoPoints = await _context.EcoPoints.FirstOrDefaultAsync(e => e.User.UserID == userId);
                            if (ecoPoints != null)
                            {
                                ecoPoints.TotalPoints += badge.BonusEcoPoints;
                                ecoPoints.AvailablePoints += badge.BonusEcoPoints;
                            }
                        }
                    }
                    else if (!qualifies && userBadge != null)
                    {
                        // Revoke badge
                        _context.UserBadge.Remove(userBadge);

                        var ecoPoints = await _context.EcoPoints.FirstOrDefaultAsync(e => e.User.UserID == userId);
                        if (ecoPoints != null && badge.BonusEcoPoints > 0)
                        {
                            ecoPoints.TotalPoints -= badge.BonusEcoPoints;
                            ecoPoints.AvailablePoints = Math.Max(0, ecoPoints.AvailablePoints - badge.BonusEcoPoints);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            // Now check meta-badges (based on number of badges unlocked)
            await EvaluateMetaBadgesAsync(user);
        }



    

    private async Task EvaluateMetaBadgesAsync(User user)
        {
            var userId = user.UserID;

            int badgesUnlocked = await _context.UserBadge.CountAsync(ub => ub.User.UserID == userId);

            var metaBadges = await _context.Badge
                .Where(b => b.CriteriaType == "BadgesUnlocked")
                .ToListAsync();

            foreach (var metaBadge in metaBadges)
            {
                bool qualifies = badgesUnlocked >= metaBadge.ThresholdValue;

                var userMetaBadge = await _context.UserBadge
                    .FirstOrDefaultAsync(ub => ub.User.UserID == userId && ub.Badge.BadgeID == metaBadge.BadgeID);

                if (qualifies && userMetaBadge == null)
                {
                    _context.UserBadge.Add(new UserBadge { User = user, Badge = metaBadge });

                    if (metaBadge.BonusEcoPoints > 0)
                    {
                        var ecoPoints = await _context.EcoPoints.FirstOrDefaultAsync(e => e.User.UserID == userId);
                        if (ecoPoints != null)
                        {
                            ecoPoints.TotalPoints += metaBadge.BonusEcoPoints;
                            ecoPoints.AvailablePoints += metaBadge.BonusEcoPoints;
                        }
                    }
                }
                else if (!qualifies && userMetaBadge != null)
                {
                    _context.UserBadge.Remove(userMetaBadge);

                    var ecoPoints = await _context.EcoPoints.FirstOrDefaultAsync(e => e.User.UserID == userId);
                    if (ecoPoints != null && metaBadge.BonusEcoPoints > 0)
                    {
                        ecoPoints.TotalPoints -= metaBadge.BonusEcoPoints;
                        ecoPoints.AvailablePoints = Math.Max(0, ecoPoints.AvailablePoints - metaBadge.BonusEcoPoints);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
