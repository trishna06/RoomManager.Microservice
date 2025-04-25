using System;

namespace RoomManager.Domain.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetDateTimeNow()
        {
            return DateTime.UtcNow;
        }
    }
}

