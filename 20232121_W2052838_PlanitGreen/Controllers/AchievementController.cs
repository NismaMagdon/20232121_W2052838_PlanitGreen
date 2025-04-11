using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Managers;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace _20232121_W2052838_PlanitGreen.Controllers
{
    public class AchievementController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly DonationManager _donationManager;

        public AchievementController(ApplicationDbContext context, DonationManager donationManager)
        {
            _context = context;
            _donationManager = donationManager;
        }

        public IActionResult Dashboard()
        {
            // Check if user is logged in by looking for UserID in the session
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                // Store the intended dashboard URL in session
                HttpContext.Session.SetString("ReturnUrl", Url.Action("Dashboard", "Achievement"));

                // Redirect to login page if not logged in
                return RedirectToAction("Login", "Account");
            }

            var user = _context.User
                .Include(u => u.UserBadge)
                    .ThenInclude(ub => ub.Badge)
                .FirstOrDefault(u => u.UserID == userId);

            var ecoPoints = _context.EcoPoints
                .FirstOrDefault(e => e.User.UserID == userId);

            var allBadges = _context.Badge.ToList();

            var earnedBadges = user.UserBadge.Select(ub => ub.Badge).ToList();
            var pendingBadges = allBadges.Except(earnedBadges).ToList();

            var leaderboard = _context.User
                .Include(u => u.UserBadge)
                .Join(_context.EcoPoints,
                    user => user.UserID,
                    eco => eco.User.UserID,
                    (user, eco) => new UserLeaderboardEntry
                    {
                        Username = user.Username,
                        TotalPoints = eco.TotalPoints,
                        BadgesEarnedCount = user.UserBadge.Count
                    })
                .OrderByDescending(e => e.TotalPoints)
                .ThenByDescending(e => e.BadgesEarnedCount)
                .Take(10)
                .ToList();

            var viewModel = new DashboardViewModel
            {
                FirstName = user.FirstName,
                AvailableEcoPoints = ecoPoints?.AvailablePoints ?? 0,
                TotalEcoPoints = ecoPoints?.TotalPoints ?? 0,
                TreesPlanted = user.TreesPlanted,
                EarnedBadges = earnedBadges,
                PendingBadges = pendingBadges,
                Leaderboard = leaderboard
            };

            return View(viewModel);
        }

        // POST: Handle donation logic using DonationManager
        [HttpPost]
        public IActionResult DonateEcoPoints(int donationAmount)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.User.FirstOrDefault(u => u.UserID == userId);

            // Ensure the user has enough eco points to donate
            if (donationAmount <= 0 || user == null)
            {
                ViewBag.ErrorMessage = "Invalid donation amount.";
                return RedirectToAction("Dashboard");
            }

            // Call the DonationManager to handle the donation logic
            var donation = _donationManager.DonateEcoPoints(user, donationAmount);

            if (donation == null)
            {
                // If donation failed (e.g., not enough eco points)
                ViewBag.ErrorMessage = "You do not have enough eco points to donate.";
                return RedirectToAction("Dashboard"); // Stay on the same page with error
            }

            // If donation is successful, redirect to confirmation page
            return RedirectToAction("DonationConfirmation", new { id = donation.DonationID });
        }

        // GET: Donation confirmation page
        public IActionResult DonationConfirmation(int id)
        {
            var donation = _context.Donation
            .Include(d => d.User)
            .FirstOrDefault(d => d.DonationID == id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);

        }
    }
}
