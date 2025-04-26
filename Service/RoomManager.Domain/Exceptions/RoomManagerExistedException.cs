namespace RoomManager.Domain.Exceptions
{
    public class RoomManagerExistedException : RoomManagerDomainException
    {
        public RoomManagerExistedException(string number) : base($"Room with number '{number}' already exist")
        { }
    }
}
