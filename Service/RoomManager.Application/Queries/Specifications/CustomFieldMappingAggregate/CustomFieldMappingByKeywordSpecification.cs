using Microservice.Utility.Application.SeedWork;
using Microservice.Utility.Domain.SeedWork;

namespace RoomManager.Application.Queries.Specifications.CustomFieldMappingAggregate
{
    public class CustomFieldMappingByKeywordSpecification : BaseSpecification<CustomFieldMapping>
    {
        public CustomFieldMappingByKeywordSpecification(int? skip,
            int? take,
            string keyword) : base(w => (string.IsNullOrWhiteSpace(keyword) ||
                                        w.OwnerType.Contains(keyword) ||
                                        w.FieldName.Contains(keyword)) &&
                                        !w.IsDeleted)
        {

            if (skip.HasValue && take.HasValue)
            {
                Page(skip.Value, take.Value);
            }
            OrderByDescending(t => t.ModifiedDateTime);
        }
    }
}
