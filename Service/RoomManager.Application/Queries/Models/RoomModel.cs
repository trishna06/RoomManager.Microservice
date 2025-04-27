namespace RoomManager.Application.Queries.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public RoomAvailabilityModel Availability { get; set; }
    }

    public class RoomAvailabilityModel
    {
        public string RoomId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string UpdatedDateTime { get; set; }
    }
}
