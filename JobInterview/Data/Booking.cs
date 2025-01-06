using System.Text.Json.Serialization;

namespace JobInterview.Data
{
    public record Booking
    {
        public required string HotelId { get; init; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public required DateOnly Arrival { get; init; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public required DateOnly Departure { get; init; }
        public required string RoomType { get; init; }
        public required string RoomRate { get; init; }

    }
}
