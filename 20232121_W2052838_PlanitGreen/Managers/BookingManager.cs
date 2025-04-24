using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;
using Microsoft.EntityFrameworkCore;

namespace _20232121_W2052838_PlanitGreen.Managers
{
    public class BookingManager
    {
        private readonly ApplicationDbContext _context;
        private readonly BadgeEvaluator _badgeEvaluator;

        public BookingManager(ApplicationDbContext context, BadgeEvaluator badgeEvaluator)
        {
            _context = context;
            _badgeEvaluator = badgeEvaluator;
        }

        public Booking CreateBooking(Departure departure, User user, int passengerCount, int redeemPoints, bool IsPublicTransport, List<Passenger> Passengers)
        {
            if (departure == null)
            {
                throw new ArgumentNullException(nameof(departure), "Departure cannot be null");
            }

            if (departure.Tour == null)
            {
                throw new ArgumentNullException(nameof(departure.Tour), "Tour information is missing for the departure");
            }

            if (departure == null || user == null)
            {
                return null;
            }

            //Computation of total price
            double initialPrice = departure.Tour.Price * passengerCount;
            double discount = redeemPoints / 10;
            double totalPrice = initialPrice - discount;

            //Computation of eco points earned
            int ecoPointsEarned = (int)(departure.Tour.CalculateEcoPoints() * passengerCount);
            if (IsPublicTransport)
            {
                ecoPointsEarned = (int)(ecoPointsEarned * 1.5);
            }

            // 1. Create booking
            var booking = new Booking
            {
                Departure = departure,
                User = user,
                PassengerQty = passengerCount,
                IsPublicTransport = IsPublicTransport,
                EcoPointsUsed = redeemPoints,
                TotalPrice = totalPrice,
            };

            _context.Booking.Add(booking);
            _context.SaveChanges();

            // 2. Create passenger instances
            foreach (var passenger in Passengers)
            {
                var passengerEntity = new Passenger
                {
                    Booking = booking,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    MealType = passenger.MealType
                };

                _context.Passenger.Add(passengerEntity);
            }

            // 3. Update eco points earned
            var ecoPoints = _context.EcoPoints.FirstOrDefault(e => e.User.UserID == user.UserID);
            if (ecoPoints != null)
            {
                ecoPoints.TotalPoints += ecoPointsEarned;
                ecoPoints.AvailablePoints += ecoPointsEarned;
                _context.EcoPoints.Update(ecoPoints);
            }

            // 4. Update trees planted
            int treesEarned = (int)(departure.Tour.TreesPlanted * passengerCount);
            user.TreesPlanted += treesEarned;

            // 5. Increase the PacksQty by the number of passengers
            departure.PacksQty += passengerCount;
            _context.Departure.Update(departure);


            _context.SaveChanges();

            // 6. Evaluate and award badges after the booking is made
            _badgeEvaluator.EvaluateBadgesAsync(user).Wait();

            return booking;
        }


        public bool CancelBooking(int bookingId)
        {
            var booking = _context.Booking
                .Include(b => b.Departure)
                .ThenInclude(d => d.Tour)
                .ThenInclude(t => t.TourStyle)
                .Include(b => b.User)
                .Include(b => b.PassengerList)
                .FirstOrDefault(b => b.BookingID == bookingId);

            if (booking == null)
                return false;

            var user = booking.User;


            // 1. Refund used eco points
            var ecoPoints = _context.EcoPoints.FirstOrDefault(e => e.User.UserID == user.UserID);
            if (ecoPoints != null)
            {
                ecoPoints.AvailablePoints += booking.EcoPointsUsed;

                // 2. Deduct earned eco points
                int ecoPointsEarned = (int)(booking.Departure.Tour.CalculateEcoPoints() * booking.PassengerQty);
                if (booking.IsPublicTransport)
                {
                    ecoPointsEarned = (int)(ecoPointsEarned * 1.5);
                }

                ecoPoints.TotalPoints -= ecoPointsEarned;
                ecoPoints.AvailablePoints -= ecoPointsEarned;

                // Make sure points don't go below zero
                ecoPoints.TotalPoints = Math.Max(ecoPoints.TotalPoints, 0);
                ecoPoints.AvailablePoints = Math.Max(ecoPoints.AvailablePoints, 0);

                _context.EcoPoints.Update(ecoPoints);
            }

            // 3. Deduct trees planted
            int treesToDeduct = (int)(booking.Departure.Tour.TreesPlanted * booking.PassengerQty);
            user.TreesPlanted -= treesToDeduct;
            if (user.TreesPlanted < 0) user.TreesPlanted = 0;

            // 4. Restore PacksQty
            booking.Departure.PacksQty -= booking.PassengerQty;
            _context.Departure.Update(booking.Departure);

            // 5. Remove passengers
            _context.Passenger.RemoveRange(booking.PassengerList);

            // 6. Remove the booking
            _context.Booking.Remove(booking);

            // 7. Save all changes
            _context.SaveChanges();

            // 8. Re-evaluate badges
            _badgeEvaluator.EvaluateBadgesAsync(user).Wait();

            return true;
        }
    }

    
}
