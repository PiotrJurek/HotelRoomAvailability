using System.Text.Json.Serialization;

namespace HotelRoomAvailability
{
    internal class Booking
    {
        public string HotelId { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
        public string RoomType { get; set; }
        public string RoomRate { get; set; }
    }
}
