namespace JobInterview.Interfaces
{
    public interface IBookingHandler
    {
        int GetBookingAvailability(DateOnly startDate, DateOnly endDate,  string roomType);
        void ImportBookings(string bookingsPath, string hotelId);

    }
}
