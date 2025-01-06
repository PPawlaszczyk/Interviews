using JobInterview;
using JobInterview.Handlers;
using JobInterview.Interfaces;

Console.WriteLine("start job interview software");


while (true)
{
    Console.WriteLine("Type any to continue");

    if (string.IsNullOrEmpty(Console.ReadLine()))
    {
        break;
    }

    try
    {
        Console.WriteLine("JobInterview software");

        IHotelHandler hotelHandler = new HotelJsonHandler(HotelsPath.Path);

        Console.WriteLine("Check availability");
        Console.WriteLine("Enter hotel Id:");

        var hotelId = Console.ReadLine()?.ToLower();
        
        if (string.IsNullOrEmpty(hotelId))
        {
            break;
        }

        IBookingHandler bookingHandler = new BookingJsonHandler(BookingsPath.Path, hotelHandler.GetHotelById(hotelId));

        Console.WriteLine("Enter the date range in one of the following formats: 'startDate endDate' yyyyMMdd-yyyyMMdd or a single date yyyyMMdd:");

        var dataRange = Console.ReadLine()?.ToLower();

        if (string.IsNullOrEmpty(dataRange))
        {
            break;
        }

        var dates = dataRange.Replace(" ", string.Empty).Split('-');

        if (dates.Length == 0 || dates.Length > 2)
        {
            Console.WriteLine("Wrong data format, please enter it again:");

            continue;
        }

        DateOnly endDate;

        if (!DateOnly.TryParseExact(dates[0], "yyyyMMdd", out var startDate))
        {
            throw new FormatException($"Invalid date format. Expected format: yyyyMMdd. Input: {dates[0]}");
        }

        endDate = startDate;

        if (dates.Length == 2 && !DateOnly.TryParseExact(dates[1], "yyyyMMdd", out endDate))
        {
            throw new FormatException($"Invalid date format. Expected format: yyyyMMdd. Input: {dates[1]}");
        }

        Console.WriteLine("Enter type of room:");

        var roomType = Console.ReadLine()?.ToLower();

        if (string.IsNullOrEmpty(roomType))
        {
            break;
        }

        Console.WriteLine($"Available rooms: {bookingHandler.GetBookingAvailability(startDate, endDate, roomType)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Something went wrong");
        Console.WriteLine(ex);
    }

}


