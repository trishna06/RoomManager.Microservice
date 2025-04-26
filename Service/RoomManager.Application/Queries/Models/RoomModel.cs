namespace RoomManager.Application.Queries.Models
{
    public class RoomModel
    {
        public string Number { get; set; }
        public string Type { get; set; }
        public RoomAvailabilityModel Availability { get; set; }
    }

    public class RoomAvailabilityModel
    {
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
