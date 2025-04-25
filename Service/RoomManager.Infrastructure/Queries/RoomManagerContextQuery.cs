using Microservice.Utility.Infrastructure;

namespace RoomManager.Infrastructure.Queries
{
    public class RoomManagerContextQuery : ContextQueryBase<RoomManagerContext>
    {
        public RoomManagerContextQuery(RoomManagerContext context) : base(context)
        {
        }
    }
}
