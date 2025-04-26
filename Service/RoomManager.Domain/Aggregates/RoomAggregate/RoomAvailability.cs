using Microservice.Utility.Domain.SeedWork;

namespace RoomManager.Domain.Aggregates.RoomAggregate
{
    public class RoomAvailability : Entity
    {
        public string Type { get; protected set; }
        public string Status { get; protected set; }

        public RoomAvailability() { }

        public RoomAvailability(string type, string status) : this()
        {
            Type = type;
            Status = status;
        }
    }
}
