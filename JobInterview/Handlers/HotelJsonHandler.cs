using JobInterview.Data;
using JobInterview.Interfaces;
using System.Text.Json;

namespace JobInterview.Handlers
{
    public class HotelJsonHandler : IHotelHandler
    {
        private JsonSerializerOptions jsonOptions;
        private IEnumerable<Hotel> hotels = [];

        public HotelJsonHandler(string hotelsPath)
        {
            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            ImportHotels(hotelsPath);
        }

        public Hotel GetHotelById(string hotelId)
        {
            return hotels.SingleOrDefault(hotels => hotels.Id.ToLower() == hotelId.ToLower())! 
                ?? throw new InvalidOperationException("Hotel not found or there are multiple hotels with the same ID.");
        }

        public void ImportHotels(string hotelsPath)
        {
            if (!File.Exists(hotelsPath))
            {
                throw new InvalidOperationException("File doesn't exsist.");
            }

            hotels = JsonSerializer.Deserialize<IEnumerable<Hotel>>(File.ReadAllText(hotelsPath), jsonOptions) ?? [];
        }
    }
}
