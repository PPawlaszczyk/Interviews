using JobInterview.Data;
using JobInterview.Handlers;
using JobInterview.Interfaces;
using JobInterviewTests.Builders;
using System.Text.Json;

namespace JobInterviewTests.Handlers
{
    public class BookingJsonHandlerTests : IDisposable
    {
        private string tempFilePath = "temporaryBookingHotelJsonFile.json";

        [Fact]
        public void GetBookingAvailability_should_return_exception()
        {
            // arrange
            
            List<Booking> bookings = [];

            HotelBuilder hotelBuilder = new HotelBuilder();
            BookingBuilder bookingBuilder = new BookingBuilder();             

            var hotel = hotelBuilder.Build();

            var booking = bookingBuilder.Build();

            bookings.Add(booking);

            var jsonBookings = JsonSerializer.Serialize(bookings);

            File.WriteAllText(tempFilePath, jsonBookings);

            IBookingHandler bookingHandler = new BookingJsonHandler(tempFilePath, hotel);

            // act && assert

            Assert.Throws<InvalidOperationException>(() => new BookingJsonHandler("notExisting.json", hotel));


            Assert.Throws<InvalidOperationException>(() =>
                         bookingHandler.GetBookingAvailability(
                             DateOnly.FromDateTime(DateTime.Now.AddDays(-3)), 
                             DateOnly.FromDateTime(DateTime.Now.AddDays(3)), 
                             "DBL"));

            Assert.Throws<InvalidOperationException>(() =>
                         bookingHandler.GetBookingAvailability(
                             DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                             DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
                             "DBL"));

            Assert.Throws<InvalidOperationException>(() =>
                         bookingHandler.GetBookingAvailability(
                             DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                             DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                             "DBL"));

            Assert.Throws<InvalidOperationException>(() =>
                         bookingHandler.GetBookingAvailability(
                             DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                             DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                             ""));
        }

        [Fact]
        public void GetBookingAvailability_should_return_zero_rooms_for_wrong_roomType()
        {
            // arrange

            List<Booking> bookings = [];

            HotelBuilder hotelBuilder = new HotelBuilder(false);
            BookingBuilder bookingBuilder = new BookingBuilder();
            bookingBuilder.WithHotelId("h1");

            hotelBuilder.AddRoomType(new RoomType { Description = "TestDesc", Code = "DBL" });
            hotelBuilder.AddRoom(new Room { RoomId = "A123", RoomType = "DBL" });
            hotelBuilder.WithId("h1");
            hotelBuilder.WithName("testName");

            var hotel = hotelBuilder.Build();

            var booking = bookingBuilder.Build();

            bookings.Add(booking);

            var jsonBookings = JsonSerializer.Serialize(bookings);

            File.WriteAllText(tempFilePath, jsonBookings);

            IBookingHandler bookingHandler = new BookingJsonHandler(tempFilePath, hotel);

            // act

            var output = bookingHandler.GetBookingAvailability(
                            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                            DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                            "DBL123");

            // assert

            Assert.Equal(0, output);
        }

        [Fact]
        public void GetBookingAvailability_should_return_zero_rooms_for_booked_rooms()
        {
            // arrange

            List<Booking> bookings = [];

            HotelBuilder hotelBuilder = new HotelBuilder(false);
            HotelBuilder otherhotelBuilder = new HotelBuilder(false);

            hotelBuilder.AddRoomType(new RoomType { Description = "TestDesc", Code = "DBL" });
            hotelBuilder.AddRoom(new Room { RoomId = "A123", RoomType = "DBL" });
            hotelBuilder.WithId("h1");
            hotelBuilder.WithName("testName");

            var hotel = hotelBuilder.Build();

            BookingBuilder bookingBuilder = new BookingBuilder();
            bookingBuilder.WithArrival(DateOnly.FromDateTime(DateTime.Now.AddDays(3)));
            bookingBuilder.WithDeparture(DateOnly.FromDateTime(DateTime.Now.AddDays(6)));
            bookingBuilder.WithRoomType("DBL");
            bookingBuilder.WithHotelId("h1");

            var booking = bookingBuilder.Build();
            bookings.Add(booking);

            BookingBuilder otherBookingBuilder = new BookingBuilder();
            otherBookingBuilder.WithArrival(DateOnly.FromDateTime(DateTime.Now.AddDays(77)));
            otherBookingBuilder.WithDeparture(DateOnly.FromDateTime(DateTime.Now.AddDays(78)));
            otherBookingBuilder.WithRoomType("DBL");
            otherBookingBuilder.WithHotelId("h2");

            var otherBooking = otherBookingBuilder.Build();
            bookings.Add(otherBooking);

            var jsonBookings = JsonSerializer.Serialize(bookings);

            File.WriteAllText(tempFilePath, jsonBookings);

            IBookingHandler bookingHandler = new BookingJsonHandler(tempFilePath, hotel);

            // act

            var output = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                        "DBL");

            var output1 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(9)),
                        "DBL");

            var output2 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                        "DBL");

            var output3 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(8)),
                        "DBL");

            var output4 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                        "DBL");

            var output5 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(6)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(6)),
                        "DBL");

            var output6 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                         "DBL");

            var output7 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(6)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                         "DBL");

            // assert

            Assert.Equal(0, output);
            Assert.Equal(0, output1);
            Assert.Equal(0, output2); 
            Assert.Equal(0, output3);
            Assert.Equal(0, output4);
            Assert.Equal(0, output5);
            Assert.Equal(0, output6);
            Assert.Equal(0, output7);
        }

        [Fact]
        public void GetBookingAvailability_should_return_rooms()
        {
            // arrange

            List<Booking> bookings = [];

            HotelBuilder hotelBuilder = new HotelBuilder(false);

            hotelBuilder.AddRoomType(new RoomType { Description = "TestDesc", Code = "DBL" });
            hotelBuilder.AddRoom(new Room { RoomId = "A123", RoomType = "DBL" });
            hotelBuilder.WithId("h1");
            hotelBuilder.WithName("testName");

            var hotel = hotelBuilder.Build();

            BookingBuilder bookingBuilder = new BookingBuilder();
            bookingBuilder.WithArrival(DateOnly.FromDateTime(DateTime.Now));
            bookingBuilder.WithDeparture(DateOnly.FromDateTime(DateTime.Now));
            bookingBuilder.WithRoomType("DBL");
            bookingBuilder.WithHotelId("h1");

            var booking = bookingBuilder.Build();
            bookings.Add(booking);

            BookingBuilder otherBookingBuilder = new BookingBuilder();
            otherBookingBuilder.WithArrival(DateOnly.FromDateTime(DateTime.Now.AddDays(3)));
            otherBookingBuilder.WithDeparture(DateOnly.FromDateTime(DateTime.Now.AddDays(5)));
            otherBookingBuilder.WithRoomType("DBL");
            otherBookingBuilder.WithHotelId("h1");

            var otherbooking = bookingBuilder.Build();
            bookings.Add(otherbooking);

            var jsonBookings = JsonSerializer.Serialize(bookings);

            File.WriteAllText(tempFilePath, jsonBookings);

            IBookingHandler bookingHandler = new BookingJsonHandler(tempFilePath, hotel);

            // act

            var output = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(6)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                        "DBL");

            var output1 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                        "DBL");

            var output2 = bookingHandler.GetBookingAvailability(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(8)),
                        DateOnly.FromDateTime(DateTime.Now.AddDays(11)),
                        "DBL");

            // assert

            Assert.Equal(1, output);
            Assert.Equal(1, output1);
            Assert.Equal(1, output2);
        }

        [Fact]
        public void GetBookingAvability_should_return_negative()
        {
            // arrange

            List<Booking> bookings = [];

            HotelBuilder hotelBuilder = new HotelBuilder(false);

            hotelBuilder.AddRoomType(new RoomType { Description = "h1", Code = "DBL" });
            hotelBuilder.AddRoom(new Room { RoomId = "A123", RoomType = "DBL" });
            hotelBuilder.WithId("h1");
            hotelBuilder.WithName("testName");

            var hotel = hotelBuilder.Build();

            BookingBuilder bookingBuilder = new BookingBuilder();
            bookingBuilder.WithArrival(DateOnly.FromDateTime(DateTime.Now));
            bookingBuilder.WithDeparture(DateOnly.FromDateTime(DateTime.Now));
            bookingBuilder.WithRoomType("DBL");
            bookingBuilder.WithHotelId("h1");

            var booking = bookingBuilder.Build();
            bookings.Add(booking);

            BookingBuilder otherBookingBuilder = new BookingBuilder();
            otherBookingBuilder.WithArrival(DateOnly.FromDateTime(DateTime.Now));
            otherBookingBuilder.WithDeparture(DateOnly.FromDateTime(DateTime.Now));
            otherBookingBuilder.WithRoomType("DBL");
            otherBookingBuilder.WithHotelId("h1");

            var otherbooking = otherBookingBuilder.Build();
            bookings.Add(otherbooking);

            var jsonBookings = JsonSerializer.Serialize(bookings);

            File.WriteAllText(tempFilePath, jsonBookings);

            IBookingHandler bookingHandler = new BookingJsonHandler(tempFilePath, hotel);

            // act

            var output = bookingHandler.GetBookingAvailability(
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now),
                "DBL");

            // assert

            Assert.Equal(-1, output);

        }

        [Fact]
        public void ImportBookings_should_throw_exception()
        {
            // arrange

            HotelBuilder hotelBuilder = new HotelBuilder();
            var hotel = hotelBuilder.Build();
            File.WriteAllText(tempFilePath, "");

            // act && assert

            Assert.Throws<InvalidOperationException>(() => new BookingJsonHandler("notExisting.json", hotel));
            Assert.Throws<JsonException>(() => new BookingJsonHandler(tempFilePath, hotel));
        }

        public void Dispose()
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

    }
}
