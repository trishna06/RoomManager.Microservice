using System;
using RoomManager.API.Helpers;
using Microservice.Utility.Domain.SeedWork;
using Microsoft.AspNetCore.Http;

namespace RoomManager.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public Guid? UserId => _contextAccessor.HttpContext?.GetCurrentUserId();

        public string Username => _contextAccessor.HttpContext?.GetCurrentUserName();
    }
}
