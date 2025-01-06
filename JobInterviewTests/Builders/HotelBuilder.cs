using Bogus;
using JobInterview.Data;

namespace JobInterviewTests.Builders
{
    public class HotelBuilder
    {
        private string id;
        private string name;
        private List<RoomType> roomTypes = [];
        private List<Room> rooms = [];
        private Faker faker;

        public HotelBuilder(bool withRandomData = true)
        {
            faker = new Faker();

            if (withRandomData)
            {
                id = faker.Random.AlphaNumeric(5);
                name = faker.Company.CompanyName();
                roomTypes = GenerateRandomRoomTypes();
                rooms = GenerateRandomRooms();
            }
            else
            {
                roomTypes = new List<RoomType>();
                rooms = new List<Room>();
            }
        }
        public HotelBuilder WithId(string id)
        {
            this.id = id;
            return this;
        }

        public HotelBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public HotelBuilder AddRoomType(RoomType roomType)
        {
            if (roomType == null)
            {
                throw new ArgumentNullException(nameof(roomType));
            }

            roomTypes.Add(roomType);
            return this;
        }

        public HotelBuilder AddRoom(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            rooms.Add(room);
            return this;
        }

        public Hotel Build()
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException("Hotel ID must be set.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("Hotel name must be set.");
            }

            return new Hotel
            {
                Id = id,
                Name = name,
                RoomTypes = roomTypes,
                Rooms = rooms
            };
        }

        private List<RoomType> GenerateRandomRoomTypes()
        {
            var roomTypes = new List<RoomType>();
            int numberOfRoomTypes = faker.Random.Int(1, 13); 

            for (int i = 0; i < numberOfRoomTypes; i++)
            {
                roomTypes.Add(new RoomType
                {
                    Code = faker.Random.ArrayElement(["DBL", "SGL"]),
                    Description = faker.Lorem.Sentence()
                });
            }

            return roomTypes;
        }

        private List<Room> GenerateRandomRooms()
        {
            var rooms = new List<Room>();
            int numberOfRooms = faker.Random.Int(5, 20); 

            for (int i = 0; i < numberOfRooms; i++)
            {
                rooms.Add(new Room
                {
                    RoomId = "R" + faker.Random.Number(0, 333), 
                    RoomType = faker.Random.ArrayElement(["DBL", "SGL"])
                });
            }

            return rooms;
        }

    }
}
