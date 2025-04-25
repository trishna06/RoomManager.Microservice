namespace RoomManager.Domain.Exceptions
{
    public class RoomArgumentException : RoomDomainException
    {
        public RoomArgumentException(string message) : base(message)
        {
        }
    }
}
