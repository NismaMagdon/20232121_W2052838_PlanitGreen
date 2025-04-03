using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;

namespace _20232121_W2052838_PlanitGreen.Managers
{
    public class BookingManager
    {
        private readonly ApplicationDbContext _context;

        public BookingManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public Booking CreateBooking(Departure departure, User user, int passengerCount, int redeemPoints, bool IsPublicTransport, List<Passenger> Passengers)
        {
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

            _context.SaveChanges();

            return booking;
        }


    }
}
