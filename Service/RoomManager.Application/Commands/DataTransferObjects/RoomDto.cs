namespace RoomManager.Application.Commands.DataTransferObjects
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public RoomAvailabilityDto Availability { get; set; }
    }

    public class RoomAvailabilityDto
    {
        public int? RoomId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string UpdatedDateTime { get; set; }
    }
}
