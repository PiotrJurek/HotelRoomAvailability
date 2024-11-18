using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace HotelRoomAvailability
{
    internal class Program
    {
        static int RoomAvailability(List<Hotel> hotels, List<Booking> bookings, Booking booking)
        {
            Hotel? hotel = hotels.FirstOrDefault(h => h.Id == booking.HotelId);
            if (hotel == null)
            {
                throw new KeyNotFoundException($"Hotel with id {booking.HotelId} does not exist.");
            }

            int rooms = hotel.Rooms.Count(r => r.RoomType == booking.RoomType);
            int bookedRooms = bookings.Where(
                b => b.HotelId == booking.HotelId &&
                b.RoomType == booking.RoomType &&
                !(int.Parse(b.Departure) <= int.Parse(booking.Arrival) ||
                int.Parse(b.Arrival) >= int.Parse(booking.Departure))).Count();

            return rooms - bookedRooms;
        }

        static Booking ParseBooking(string text)
        {
            string pattern = @"Availability\(([^,]+),\s*([0-9]+)(?:-([0-9]+))?,\s*([^)]+)\)";
            Regex regex = new Regex(pattern);
            Booking output = new Booking();

            Match match = regex.Match(text);

            if (!match.Success)
            {
                throw new FormatException("String could not be parsed to Booking type.");
            }

            output.HotelId = match.Groups[1].Value.Trim();
            output.Arrival = match.Groups[2].Value;
            output.Departure = output.Arrival;
            if (match.Groups[3].Success)
            {
                output.Departure = match.Groups[3].Value;
            }
            output.RoomType = match.Groups[4].Value.Trim();

            return output;
        }

        static void Main(string[] args)
        {
            int hotelsArgIndex = Array.IndexOf(args, "--hotels") + 1;
            int bookingsArgIndex = Array.IndexOf(args, "--bookings") + 1;

            if(hotelsArgIndex <= 0 || hotelsArgIndex >= args.Length)
            {
                Console.WriteLine("Missing or incorrectly declared --hotels argument.");
                return;
            }

            if (bookingsArgIndex <= 0 || bookingsArgIndex >= args.Length)
            {
                Console.WriteLine("Missing or incorrectly declared --bookings argument.");
                return;
            }

            string hotelsJson = File.ReadAllText(args[hotelsArgIndex]);
            string bookingsJson = File.ReadAllText(args[bookingsArgIndex]);

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
            List<Hotel>? hotels = JsonSerializer.Deserialize<List<Hotel>>(hotelsJson, options);
            List<Booking>? bookings = JsonSerializer.Deserialize<List<Booking>>(bookingsJson, options);

            if(hotels == null)
            {
                Console.WriteLine($"Failed to parse {args[hotelsArgIndex]} file.");
                return;
            }

            if (bookings == null)
            {
                Console.WriteLine($"Failed to parse {args[bookingsArgIndex]} file.");
                return;
            }

            while (true)
            {
                string? input = Console.ReadLine();
                if(input == null || input == "")
                {
                    break;
                }

                try
                {
                    Booking booking = ParseBooking(input);

                    int rooms = RoomAvailability(hotels, bookings, booking);

                    Console.WriteLine($"Number of available rooms: {rooms}");
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }

            return;
        }
    }
}
