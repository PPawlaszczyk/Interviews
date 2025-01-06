using Bogus;
using JobInterview.Data;

public class BookingBuilder
{
    private string hotelId;
    private DateOnly arrival;
    private DateOnly departure;
    private string roomType;
    private string roomRate;
    private Faker faker;

    public BookingBuilder()
    {
        faker = new Faker();

        hotelId = faker.Random.AlphaNumeric(5);
        arrival = DateOnly.FromDateTime(DateTime.Today);
        departure = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        roomType = faker.Random.ArrayElement(["DBL", "SGL"]);
        roomRate = faker.Random.ArrayElement(["Standard", "Prepaid"]);
    }

    public BookingBuilder WithHotelId(string hotelId)
    {
        this.hotelId = hotelId;
        return this;
    }

    public BookingBuilder WithArrival(DateOnly arrival)
    {
        this.arrival = arrival;
        return this;
    }

    public BookingBuilder WithDeparture(DateOnly departure)
    {
        this.departure = departure;
        return this;
    }

    public BookingBuilder WithRoomType(string roomType)
    {
        this.roomType = roomType;
        return this;
    }

    public BookingBuilder WithRoomRate(string roomRate)
    {
        this.roomRate = roomRate;
        return this;
    }

    public Booking Build()
    {
        if (string.IsNullOrEmpty(hotelId))
        {
            throw new InvalidOperationException("HotelId must be provided.");
        }

        if (arrival == default)
        {
            throw new InvalidOperationException("Arrival date must be provided.");
        }

        if (departure == default)
        {
            throw new InvalidOperationException("Departure date must be provided.");
        }

        if (string.IsNullOrEmpty(roomType))
        {
            throw new InvalidOperationException("RoomType must be provided.");
        }

        if (string.IsNullOrEmpty(roomRate))
        {
            throw new InvalidOperationException("RoomRate must be provided.");
        }

        return new Booking
        {
            HotelId = hotelId,
            Arrival = arrival,
            Departure = departure,
            RoomType = roomType,
            RoomRate = roomRate
        };
    }
}