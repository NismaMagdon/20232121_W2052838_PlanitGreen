using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Managers;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20232121_W2052838_PlanitGreen.Tests
{
    [TestClass]
    public class BadgeEvaluatorTests
    {
        private ApplicationDbContext _context;
        private BadgeEvaluator _badgeEvaluator;


        [TestInitialize]
        public void Setup()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _badgeEvaluator = new BadgeEvaluator(_context);
        }



        //Test for when badge criteria is met
        [TestMethod]
        public async Task EvaluateBadgesAsync_GrantsBadge_WhenCriteriaMet()
        {
            // Arrange: Create a user
            var user = new User
            {
                UserID = 78,
                FirstName = "Jane",
                LastName = "Doe",
                Dob = new DateTime(1995, 5, 10),
                Username = "janedoe",
                Password = "password123",
                Role = Role.Traveller
            };
            _context.User.Add(user);

            // Create a Destination and TourStyle for the Tour
            var destination = new Destination
            {
                DestinationID = 78,
                DestinationName = "India"
            };
            _context.Destination.Add(destination);

            var tourStyle = new TourStyle
            {
                TourStyleID = 78,
                TourStyleName = "Eco-Tour",
                StyleDescription = "Sustainable and eco-friendly tour"
            };
            _context.TourStyle.Add(tourStyle);

            // Add a Tour for the Departure
            var tour = new Tour
            {
                TourID = 78,
                TourName = "Eco Jungle Adventure",
                Description = "Explore the beauty of nature while staying eco-friendly",
                TourStyle = tourStyle,
                Destination = destination,
                Duration = 7,
                Price = 500,
                CarbonFootprint = 30,
                TreesPlanted = 50,
                IsActive = true
            };
            _context.Tour.Add(tour);


            // Add 2 Departures for the tour
            var departure1 = new Departure
            {
                DepartureID = 78,
                Tour = tour,
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                PacksLimit = 20,
                PacksQty = 0,
                Iscancelled = false
            };
            var departure2 = new Departure
            {
                DepartureID = 79,
                Tour = tour,
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(20)),
                PacksLimit = 20,
                PacksQty = 0,
                Iscancelled = false
            };
            _context.Departure.AddRange(departure1, departure2);

            // Add 3 bookings for this user (mix of public transport and not)
            _context.Booking.AddRange(
                new Booking
                {
                    BookingID = 78,
                    User = user,
                    Departure = departure1,
                    PassengerQty = 1,
                    IsPublicTransport = true,
                    EcoPointsUsed = 10,
                    TotalPrice = 490
                },
                new Booking
                {
                    BookingID = 79,
                    User = user,
                    Departure = departure2,
                    PassengerQty = 2,
                    IsPublicTransport = false,
                    EcoPointsUsed = 0,
                    TotalPrice = 1000
                },
                new Booking
                {
                    BookingID = 80,
                    User = user,
                    Departure = departure1,
                    PassengerQty = 1,
                    IsPublicTransport = true,
                    EcoPointsUsed = 20,
                    TotalPrice = 480
                }
            );

            // Add a badge that requires 2 public transport bookings
            var badge = new Badge
            {
                BadgeID = 78,
                BadgeName = "Public Transport Pro",
                BadgeDescription = "Awarded for booking two or more public transport tours.",
                BadgeCategory = "Sustainability",
                CriteriaType = "PublicTransportCount",
                ThresholdValue = 2,
                BadgeImage = "public_transport_pro.png",
                BonusEcoPoints = 50
            };
            _context.Badge.Add(badge);

            // Add EcoPoints record
            var ecoPoints = new EcoPoints
            {
                PointsID = 78,
                User = user,
                TotalPoints = 0,
                AvailablePoints = 0
            };
            _context.EcoPoints.Add(ecoPoints);
            await _context.SaveChangesAsync();

            // Act
            await _badgeEvaluator.EvaluateBadgesAsync(user);

            // Assert
            var userBadge = _context.UserBadge
        .Include(ub => ub.User)    // Eagerly load the related User
        .Include(ub => ub.Badge)   // Eagerly load the related Badge
        .FirstOrDefault(ub => ub.User.UserID == 78 && ub.Badge.BadgeID == 78);
            Assert.IsNotNull(userBadge, "Badge was not granted to the user");
            Assert.AreEqual(50, ecoPoints.TotalPoints);
            Assert.AreEqual(50, ecoPoints.AvailablePoints);
        }





        //Test for when badge criteria is NOT met
        [TestMethod]
        public async Task EvaluateBadgesAsync_DoesNotGrantBadge_WhenCriteriaNotMet()
        {
            // Arrange: Create a user
            var user = new User
            {
                UserID = 56,
                FirstName = "John",
                LastName = "Smith",
                Dob = new DateTime(1990, 1, 1),
                Username = "johnsmith",
                Password = "password456",
                Role = Role.Traveller
            };
            _context.User.Add(user);

            // Create supporting data
            var destination = new Destination
            {
                DestinationID = 56,
                DestinationName = "Thailand"
            };
            _context.Destination.Add(destination);

            var tourStyle = new TourStyle
            {
                TourStyleID = 56,
                TourStyleName = "Culture Trip",
                StyleDescription = "Explore heritage and culture"
            };
            _context.TourStyle.Add(tourStyle);

            var tour = new Tour
            {
                TourID = 56,
                TourName = "Temple Journey",
                Description = "Cultural exploration of ancient temples",
                TourStyle = tourStyle,
                Destination = destination,
                Duration = 5,
                Price = 600,
                CarbonFootprint = 40,
                TreesPlanted = 30,
                IsActive = true
            };
            _context.Tour.Add(tour);

            var departure = new Departure
            {
                DepartureID = 56,
                Tour = tour,
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                PacksLimit = 15,
                PacksQty = 0,
                Iscancelled = false
            };
            _context.Departure.Add(departure);

            // Only one booking with public transport (criteria not met)
            _context.Booking.AddRange(
                new Booking
                {
                    BookingID = 56,
                    User = user,
                    Departure = departure,
                    PassengerQty = 1,
                    IsPublicTransport = true, 
                    EcoPointsUsed = 0,
                    TotalPrice = 600
                },
                new Booking
                {
                    BookingID = 57,
                    User = user,
                    Departure = departure,
                    PassengerQty = 1,
                    IsPublicTransport = false, 
                    EcoPointsUsed = 0,
                    TotalPrice = 600
                }
            );

            var badge = new Badge
            {
                BadgeID = 56,
                BadgeName = "Public Transport Pro",
                BadgeDescription = "Awarded for booking two or more public transport tours.",
                BadgeCategory = "Sustainability",
                CriteriaType = "PublicTransportCount",
                ThresholdValue = 2,
                BadgeImage = "public_transport_pro.png",
                BonusEcoPoints = 20
            };
            _context.Badge.Add(badge);

            var ecoPoints = new EcoPoints
            {
                PointsID = 56,
                User = user,
                TotalPoints = 0,
                AvailablePoints = 0
            };
            _context.EcoPoints.Add(ecoPoints);

            await _context.SaveChangesAsync();

            // Act
            await _badgeEvaluator.EvaluateBadgesAsync(user);

            // Assert
            var userBadge = _context.UserBadge
    .Include(ub => ub.User)    // Eagerly load the related User
    .Include(ub => ub.Badge)   // Eagerly load the related Badge
    .FirstOrDefault(ub => ub.User.UserID == 56 && ub.Badge.BadgeID == 56);
            Assert.IsNull(userBadge, "Badge should not be assigned.");
            Assert.AreEqual(0, ecoPoints.TotalPoints);
            Assert.AreEqual(0, ecoPoints.AvailablePoints);

        }






        //Test for when user has already earned the badge
        [TestMethod]
        public async Task EvaluateBadgesAsync_DoesNotGrantBadge_WhenUserAlreadyHasIt()
        {
            // Arrange: Create a user
            var user = new User
            {
                UserID = 44,
                FirstName = "Jane",
                LastName = "Doe",
                Dob = new DateTime(1995, 5, 10),
                Username = "janedoe",
                Password = "password123",
                Role = Role.Traveller
            };
            _context.User.Add(user);

            // Create a Destination and TourStyle for the Tour
            var destination = new Destination
            {
                DestinationID = 44,
                DestinationName = "Italy"
            };
            _context.Destination.Add(destination);

            var tourStyle = new TourStyle
            {
                TourStyleID = 44,
                TourStyleName = "Eco Adventure",
                StyleDescription = "Explore responsibly"
            };
            _context.TourStyle.Add(tourStyle);

            // Add a Tour for the Departure
            var tour = new Tour
            {
                TourID = 44,
                TourName = "Green Mountain Hike",
                Description = "Eco-conscious hiking tour",
                TourStyle = tourStyle,
                Destination = destination,
                Duration = 5,
                Price = 400,
                CarbonFootprint = 25,
                TreesPlanted = 20,
                IsActive = true
            };
            _context.Tour.Add(tour);

            // Add Departures
            var departure1 = new Departure
            {
                DepartureID = 44,
                Tour = tour,
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(8)),
                PacksLimit = 10,
                PacksQty = 0,
                Iscancelled = false
            };
            var departure2 = new Departure
            {
                DepartureID = 45,
                Tour = tour,
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(12)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(17)),
                PacksLimit = 10,
                PacksQty = 0,
                Iscancelled = false
            };
            _context.Departure.AddRange(departure1, departure2);

            // Add Bookings
            _context.Booking.AddRange(
                new Booking
                {
                    BookingID = 44,
                    User = user,
                    Departure = departure1,
                    PassengerQty = 1,
                    IsPublicTransport = true,
                    EcoPointsUsed = 0,
                    TotalPrice = 400
                },
                new Booking
                {
                    BookingID = 45,
                    User = user,
                    Departure = departure2,
                    PassengerQty = 2,
                    IsPublicTransport = false,
                    EcoPointsUsed = 0,
                    TotalPrice = 800
                }
            );

            // Add a BookingCount badge that requires 2 bookings
            var badge = new Badge
            {
                BadgeID = 44,
                BadgeName = "Booking Beginner",
                BadgeDescription = "Awarded for making two bookings.",
                BadgeCategory = "Activity",
                CriteriaType = "BookingCount",
                ThresholdValue = 2,
                BadgeImage = "booking_beginner.png",
                BonusEcoPoints = 15
            };
            _context.Badge.Add(badge);

            // Add EcoPoints record
            var ecoPoints = new EcoPoints
            {
                PointsID = 44,
                User = user,
                TotalPoints = 0,
                AvailablePoints = 0
            };
            _context.EcoPoints.Add(ecoPoints);

            // Pre-grant the badge to simulate the user already has it
            var userBadge = new UserBadge
            {
                User = user,
                Badge = badge,
            };
            _context.UserBadge.Add(userBadge);

            await _context.SaveChangesAsync();

            // Act
            await _badgeEvaluator.EvaluateBadgesAsync(user);

            // Assert
            var allUserBadges = _context.UserBadge
                .Include(ub => ub.User)    // Eagerly load the related User
    .Include(ub => ub.Badge)   // Eagerly load the related Badge
                .Where(ub => ub.User.UserID == 44 && ub.Badge.BadgeID == 44)
                .ToList();
            Assert.AreEqual(1, allUserBadges.Count, "Duplicate badge should not be granted.");
            Assert.AreEqual(0, ecoPoints.TotalPoints, "Eco points should not increase.");
            Assert.AreEqual(0, ecoPoints.AvailablePoints, "Eco points should not increase.");

        }



        //Test for when badge count badges are unlocked
        [TestMethod]
        public async Task EvaluateBadgesAsync_GrantsBadgeUnlocked_WhenBadgeCountReached()
        {
            // Arrange: Create a user
            var user = new User
            {
                UserID = 33,
                FirstName = "Ava",
                LastName = "Lee",
                Dob = new DateTime(1998, 3, 15),
                Username = "avaleee",
                Password = "securepass",
                Role = Role.Traveller
            };
            _context.User.Add(user);

            // Add existing badges the user already unlocked
            var existingBadges = new List<Badge>();
            for (int i = 33; i <= 35; i++)
            {
                var badge = new Badge
                {
                    BadgeID = i,
                    BadgeName = $"Sample Badge {i}",
                    BadgeDescription = "Dummy badge",
                    BadgeCategory = "Misc",
                    CriteriaType = "Dummy",
                    ThresholdValue = 0,
                    BadgeImage = $"badge{i}.png",
                    BonusEcoPoints = 5
                };
                existingBadges.Add(badge);
                _context.Badge.Add(badge);
                _context.UserBadge.Add(new UserBadge
                {


                    User = user,
                    Badge = badge,
                });
            }

            // Add the BadgesUnlocked badge that requires unlocking 3 badges
            var unlockBadge = new Badge
            {
                BadgeID = 36,
                BadgeName = "Badge Collector",
                BadgeDescription = "Unlock 3 badges to earn this.",
                BadgeCategory = "Milestone",
                CriteriaType = "BadgesUnlocked",
                ThresholdValue = 3,
                BadgeImage = "badge_collector.png",
                BonusEcoPoints = 15
            };
            _context.Badge.Add(unlockBadge);

            // EcoPoints record
            var ecoPoints = new EcoPoints
            {
                PointsID = 33,
                User = user,
                TotalPoints = 0,
                AvailablePoints = 0
            };
            _context.EcoPoints.Add(ecoPoints);

            await _context.SaveChangesAsync();

            // Act
            await _badgeEvaluator.EvaluateBadgesAsync(user);

            // Assert: Badge should be granted
            var userBadge = _context.UserBadge.Include(ub => ub.User)    // Eagerly load the related User
    .Include(ub => ub.Badge)   // Eagerly load the related Badge
    .FirstOrDefault(ub => ub.User.UserID == 33 && ub.Badge.BadgeID == 36);
            Assert.IsNotNull(userBadge, "Badge Collector was not assigned.");
            Assert.AreEqual(15, ecoPoints.TotalPoints);
        }




        [TestCleanup]
        public void Cleanup()
        {
            Console.WriteLine("Test cleanup started...");

            _context.Database.EnsureDeleted();
            _context.Dispose();

            Console.WriteLine("Test cleanup completed.");
        }


    }
}
