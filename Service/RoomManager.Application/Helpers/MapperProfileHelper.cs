using AutoMapper;
using AutoMapper.Internal;
using RoomManager.Application.Queries.Models;
using Microservice.Utility.Application.Interfaces;
using Microservice.Utility.Domain.SeedWork;

namespace RoomManager.Application.Helpers
{
    public class MapperProfileHelper : Profile
    {
        public MapperProfileHelper()
        {
            this.Internal().ForAllMaps((map, expr) => expr.AfterMap<MapCustomFieldAction>());

            CreateCustomFieldMapper();

            // AutoMapper Mappings
        }

        public void CreateCustomFieldMapper()
        {
            CreateMap<CustomFieldMapping, CustomFieldMappingModel>().ReverseMap();
            CreateMap<InputDetail, InputDetailModel>().ReverseMap();
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
