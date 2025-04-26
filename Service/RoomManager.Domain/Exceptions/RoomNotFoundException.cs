namespace RoomManager.Domain.Exceptions
{
    public class RoomNotFoundException : RoomManagerDomainException
    {
        public RoomNotFoundException(int id) : base($"Room with id '{id}' not found")
        { }
    }
}
