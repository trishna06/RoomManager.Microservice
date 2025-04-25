using System;
using Microservice.Utility.Exception;

namespace RoomManager.Domain.Exceptions
{
    public class RoomDomainException : ArcstoneException
    {
        public RoomDomainException() : base()
        { }

        public RoomDomainException(string message)
            : base(message)
        { }

        public RoomDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
