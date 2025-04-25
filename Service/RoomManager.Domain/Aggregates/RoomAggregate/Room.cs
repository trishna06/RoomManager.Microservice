using System;
using Microservice.Utility.Domain.SeedWork;

namespace RoomManager.Domain.Aggregates.RoomAggregate
{
    public class Room : Entity, IAggregate
    {
        public string Number { get; set; }
        public string Type { get; set; }

        public Room()
        {

        }

        public Room(string number, string type, DateTime updatedAt): this()
        {
            Number = number;
            Type = type;
        }
    }
}
