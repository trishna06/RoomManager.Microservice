using AutoMapper;
using AutoMapper.Internal;
using Microservice.Utility.Application.Interfaces;
using Microservice.Utility.Domain.SeedWork;
using RoomManager.Application.Queries.Models;
using RoomManager.Domain.Aggregates.RoomAggregate;

namespace RoomManager.Application.Helpers
{
    public class MapperProfileHelper : Profile
    {
        public MapperProfileHelper()
        {
            this.Internal().ForAllMaps((map, expr) => expr.AfterMap<MapCustomFieldAction>());

            CreateMap<Room, RoomModel>()
                .ReverseMap();

            CreateMap<RoomAvailability, RoomAvailabilityModel>()
                .ReverseMap();
        }
    }

    /// <summary>
    /// For auto mapping of custom fields (if any)
    /// </summary>
    public class MapCustomFieldAction : IMappingAction<object, object>
    {
        public void Process(object source, object destination, ResolutionContext context)
        {
            if (source is EntityPlus entity && destination is ICustomField cf)
            {
                cf.CustomFields = entity.CustomFields;
            }
        }
    }
}
