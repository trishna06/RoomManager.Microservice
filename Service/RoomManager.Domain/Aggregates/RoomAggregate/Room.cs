using Microservice.Utility.Domain.SeedWork;

namespace RoomManager.Domain.Aggregates.RoomAggregate
{
    public class Room : Entity, IAggregate
    {
        public string Number { get; protected set; }
        public string Type { get; protected set; }
        public RoomAvailability Availability { get; protected set; }

        public Room()
        {

        }

        public Room(string number, string type) : this()
        {
            Number = number;
            Type = type;
            Availability = new RoomAvailability("Guest", "Available");
        }

        public void Update(string number, string type)
        {
            Number = number;
            Type = type;
        }

        public void UpdateAvailability(string type, string status)
        {
            Availability = new RoomAvailability(type, status);
        }
    }
}
