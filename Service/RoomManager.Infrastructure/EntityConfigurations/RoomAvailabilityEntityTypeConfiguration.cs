using Microservice.Utility.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RoomManager.Infrastructure.EntityConfigurations
{
    public class RoomAvailabilityEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.Aggregates.RoomAggregate.RoomAvailability>
    {
        public override void Configure(EntityTypeBuilder<Domain.Aggregates.RoomAggregate.RoomAvailability> builder)
        {
            base.Configure(builder);
        }
    }
}
