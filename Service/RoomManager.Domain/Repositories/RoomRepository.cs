using Microservice.Utility.Domain.SeedWork;

namespace RoomManager.Domain.Repositories
{
    public interface IRoomRepository : IRepository<Aggregates.RoomAggregate.Room>
    {
    }
}
