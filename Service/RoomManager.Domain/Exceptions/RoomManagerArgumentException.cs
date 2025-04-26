namespace RoomManager.Domain.Exceptions
{
    public class RoomManagerArgumentException : RoomManagerDomainException
    {
        public RoomManagerArgumentException(string message) : base(message)
        {
        }
    }
}
