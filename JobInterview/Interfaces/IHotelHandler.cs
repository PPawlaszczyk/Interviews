using JobInterview.Data;

namespace JobInterview.Interfaces
{
    public interface IHotelHandler
    {
        void ImportHotels(string hotelsPath);
        Hotel GetHotelById(string hotelId);
    }
}
