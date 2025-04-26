using Microservice.Utility.Application.SeedWork;
using RoomManager.Domain.Aggregates.RoomAggregate;

namespace ContentManager.Application.Queries.Specifications.ContentAggregate
{
    public sealed class RoomSpecification : BaseSpecification<Room>
    {
        public RoomSpecification() : base(_ => true)
        {
            Include($"{nameof(Room.Availability)}");
        }
    }
}
