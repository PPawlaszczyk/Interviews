using JobInterview.Data;
using JobInterview.Handlers;
using JobInterviewTests.Builders;
using System.Text.Json;

namespace JobInterviewTests.Handlers
{
    public class HotelJsonHandlerTests : IDisposable
    {
        private string tempFilePath = "temporaryTestHotelJsonFile.json";

        [Fact]
        public void GetHotelById_should_throw_exception()
        {
            // arrange

            List<Hotel> hotels = [];

            HotelBuilder hotelbuilder = new HotelBuilder();
            hotels.Add(hotelbuilder.Build());

            var jsonHotels = JsonSerializer.Serialize(hotels);

            File.WriteAllText(tempFilePath, jsonHotels);

            HotelJsonHandler hotelJsonHandler = new HotelJsonHandler(tempFilePath);

            // act && assert

            Assert.Throws<InvalidOperationException>(() => hotelJsonHandler.GetHotelById("h1"));
        }

        [Fact]
        public void GetHotelById_should_return_hotel()
        {
            // arrange

            List<Hotel> hotels = [];

            HotelBuilder firstHotelBuilder = new HotelBuilder(false);
            
            firstHotelBuilder.WithId("h1");
            firstHotelBuilder.WithName("Test hotel");
            firstHotelBuilder.AddRoom(new Room { RoomId = "A123", RoomType = "SGL" });
            firstHotelBuilder.AddRoomType(new RoomType { Code = "SGL", Description = "Test" });

            hotels.Add(firstHotelBuilder.Build());

            HotelBuilder otherHotelBuilder = new HotelBuilder();

            hotels.Add(otherHotelBuilder.Build());

            var jsonHotels = JsonSerializer.Serialize(hotels);

            File.WriteAllText(tempFilePath, jsonHotels);

            HotelJsonHandler hotelJsonHandler = new HotelJsonHandler(tempFilePath);

            // act

            var hotel = hotelJsonHandler.GetHotelById("h1");

            // assert

            Assert.NotNull(hotel);
            Assert.Equal("h1", hotel.Id);
            Assert.Equal("Test hotel", hotel.Name);
            Assert.True(hotel.RoomTypes.Count == 1);
            Assert.True(hotel.Rooms.Count == 1);
            Assert.True(hotel.RoomTypes?.FirstOrDefault()?.Code == "SGL");
            Assert.True(hotel.RoomTypes?.FirstOrDefault()?.Description == "Test");
            Assert.True(hotel.Rooms?.FirstOrDefault()?.RoomId == "A123");
            Assert.True(hotel.Rooms?.FirstOrDefault()?.RoomType == "SGL");
        }

        [Fact]
        public void ImportBookings_should_throw_exception()
        {
            // arrange

            File.WriteAllText(tempFilePath, "");

            // act && assert

            Assert.Throws<InvalidOperationException>(() => new HotelJsonHandler("notExisting.json"));
            Assert.Throws<JsonException>(() => new HotelJsonHandler(tempFilePath));

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
