using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RoomManager.API.Helpers
{
    public static class HttpContextHelper
    {
        public static Guid? GetCurrentUserId(this HttpContext context)
        {
            string userClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userClaim))
                return null;

            if (Guid.TryParse(userClaim, out Guid userId))
                return userId;

            throw new ArgumentException("User Id is not valid Guid type.");
        }

        public static string GetCurrentUserName(this HttpContext context)
        {
            string name = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            return name;
        }
    }
}
