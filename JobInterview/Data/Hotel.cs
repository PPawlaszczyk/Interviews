namespace JobInterview.Data
{
    public record Hotel
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
        public required List<RoomType> RoomTypes { get; init; } = [];
        public required List<Room> Rooms { get; init; } = [];
    }
}
