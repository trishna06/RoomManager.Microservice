using Microservice.Utility.Infrastructure.EntityConfigurations;
using Microservice.Utility.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RoomManager.Infrastructure.EntityConfigurations
{
    public class RoomEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.Aggregates.RoomAggregate.Room>
    {
        public override void Configure(EntityTypeBuilder<Domain.Aggregates.RoomAggregate.Room> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Availability)
                   .WithMany()
                   .HasDefaultForeignKey()
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
