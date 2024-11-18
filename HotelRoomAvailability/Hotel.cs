using System.Text.Json.Serialization;

namespace HotelRoomAvailability
{
    internal class Hotel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
