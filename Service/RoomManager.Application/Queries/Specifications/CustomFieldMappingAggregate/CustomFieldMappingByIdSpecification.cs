using Microservice.Utility.Application.SeedWork;
using Microservice.Utility.Domain.SeedWork;

namespace RoomManager.Application.Queries.Specifications.CustomFieldMappingAggregate
{
    public class CustomFieldMappingByIdSpecification : BaseSpecification<CustomFieldMapping>
    {
        public CustomFieldMappingByIdSpecification(int id) : base(w => w.Id == id)
        {
        }
    }
}
