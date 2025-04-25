namespace RoomManager.Domain.Exceptions
{
    public class RoomExistedException : RoomDomainException
    {
        public RoomExistedException(string number) : base($"Room with number '{number}' already exist")
        { }
    }
}
