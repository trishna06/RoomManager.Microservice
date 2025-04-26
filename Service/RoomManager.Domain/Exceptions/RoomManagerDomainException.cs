using System;
using Microservice.Utility.Exception;

namespace RoomManager.Domain.Exceptions
{
    public class RoomManagerDomainException : ArcstoneException
    {
        public RoomManagerDomainException() : base()
        { }

        public RoomManagerDomainException(string message)
            : base(message)
        { }

        public RoomManagerDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
