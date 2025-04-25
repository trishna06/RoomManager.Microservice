using Microservice.Utility.Domain.SeedWork;

namespace RoomManager.Domain.Aggregates.RoomAggregate
{
    public class RoomAvailability: Entity, IAggregate
    {
        public Room Room { get; set; }

        public string Status { get; set; }

        public RoomAvailability() { }

        public RoomAvailability(Room room, string status): this()
        {
            Room = room;
            Status = status;
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }
    }
}
