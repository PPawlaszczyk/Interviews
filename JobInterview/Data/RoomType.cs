namespace JobInterview.Data
{
    public record RoomType
    {
        public required string Code { get; init; }
        public required string Description { get; init; }
    }

}