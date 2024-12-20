﻿using System.Text.Json.Serialization;

namespace HotelRoomAvailability
{
    public class RoomType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Features { get; set; }
    }
}