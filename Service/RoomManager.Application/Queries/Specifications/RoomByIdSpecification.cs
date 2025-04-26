using Microservice.Utility.Application.SeedWork;
using RoomManager.Domain.Aggregates.RoomAggregate;

namespace ContentManager.Application.Queries.Specifications.ContentAggregate
{
    public sealed class RoomByIdSpecification : BaseSpecification<Room>
    {
        public RoomByIdSpecification(int id) : base(room => room.Id == id)
        {
            Include($"{nameof(Room.Availability)}");
        }
    }
}
