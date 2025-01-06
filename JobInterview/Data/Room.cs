namespace JobInterview.Data
{
    public record Room
    {
        public required string RoomType { get; init; }
        public required string RoomId { get; init; }
    }
}
