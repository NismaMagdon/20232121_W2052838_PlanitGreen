using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;

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

            double initialPrice = departure.Tour.Price * passengerCount;
            double discount = redeemPoints / 10;
            double totalPrice = initialPrice - discount;

            int ecoPointsEarned = (int)(departure.Tour.CalculateEcoPoints() * passengerCount);
            if (IsPublicTransport)
            {
                ecoPointsEarned = (int)(ecoPointsEarned * 1.5);
            }

            //Create booking
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

            //Create passenger instances
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

            //Update eco points earned
            var ecoPoints = _context.EcoPoints.FirstOrDefault(e => e.User.UserID == user.UserID);
            if (ecoPoints != null)
            {
                ecoPoints.TotalPoints += ecoPointsEarned;
                ecoPoints.AvailablePoints += ecoPointsEarned;
                _context.EcoPoints.Update(ecoPoints);
            }

            //Update trees planted
            int treesEarned = (int)(departure.Tour.TreesPlanted * passengerCount);
            user.TreesPlanted += treesEarned;

            // Reduce the PacksQty by the number of passengers
            departure.PacksQty += passengerCount;

            // Update the departure's PacksQty in the database
            _context.Departure.Update(departure);


            _context.SaveChanges();

            // Evaluate and award badges after the booking is made
            _badgeEvaluator.EvaluateBadgesAsync(user).Wait();

            return booking;
        }


    }
}
