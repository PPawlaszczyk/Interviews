using ScreenshotService;

namespace JobInterview
{
    public class HotelsPath : PathApp
    {
        public static string Path
        {
            get
            {
                if (!File.Exists(BaseDirectory))
                {
                    Directory.CreateDirectory(BaseDirectory);
                }

                return System.IO.Path.Combine(BaseDirectory, "hotels.json");
            }
        }
    }

    public class BookingsPath : PathApp
    {
        public static string Path
        {
            get
            {
                if (!File.Exists(BaseDirectory))
                {
                    Directory.CreateDirectory(BaseDirectory);
                }

                return System.IO.Path.Combine(BaseDirectory, "bookings.json");
            }
        }
    }
}
