using JobInterview.Data;
using JobInterview.Interfaces;
using System.Text.Json;

namespace JobInterview.Handlers
{
    public class BookingJsonHandler : IBookingHandler
    {
        private JsonSerializerOptions jsonOptions;
        private IEnumerable<Booking> bookings = [];
        private Hotel hotel;
        public BookingJsonHandler(string bookingsPath, Hotel hotel)
        {
            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ImportBookings(bookingsPath, hotel.Id);

            this.hotel = hotel ?? throw new InvalidOperationException("hotel cannot be null.");
        }

        private int CheckAvability(DateOnly startDate, DateOnly endDate, string roomType)
        {
            int numberOfRooms = hotel.Rooms.Where(x => x.RoomType.ToLower() == roomType.ToLower()).Count();

            if (numberOfRooms == 0) return 0;

            foreach (var booking in bookings)
            {
                if (booking.RoomType.ToLower() != roomType.ToLower())
                {
                    continue;
                }

                if (booking.Arrival > startDate && booking.Arrival > endDate ||
                    booking.Departure < startDate && booking.Departure < endDate)
                {
                    continue;
                }
                numberOfRooms--;

            }

            return numberOfRooms;
        }

        public int GetBookingAvailability(DateOnly startDate, DateOnly endDate, string roomType)
        {
            if (endDate < startDate)
            {
                throw new InvalidOperationException("Start date cannot be later than end date.");

            }

            if (startDate < DateOnly.FromDateTime(DateTime.Now) || endDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new InvalidOperationException("Start date and End date cannot be in the past.");

            }

            if (string.IsNullOrEmpty(roomType))
            {
                throw new InvalidOperationException("Room Type cannot be null or empty.");
            }

            if (!hotel.RoomTypes.Any(hotel => hotel.Code.ToLower() == roomType.ToLower()) ||
                !hotel.Rooms.Any(hotel => hotel.RoomType.ToLower() == roomType.ToLower()))
            {
                return 0;
            }

            return CheckAvability(startDate, endDate, roomType);
        }

        public void ImportBookings(string bookingsPath, string hotelId)
        {
            if (!File.Exists(bookingsPath))
            {
                throw new InvalidOperationException("File doesn't exsist.");
            }

            if (string.IsNullOrEmpty(hotelId))
            {
                throw new InvalidOperationException("hotelId and roomType cannot be null or empty.");
            }

            var readBookings = JsonSerializer.Deserialize<IEnumerable<Booking>>(File.ReadAllText(bookingsPath), jsonOptions) ?? [];

            bookings = readBookings.Where(bookingObj => bookingObj.HotelId.ToLower() == hotelId.ToLower()).ToList();
        }

    }
}
