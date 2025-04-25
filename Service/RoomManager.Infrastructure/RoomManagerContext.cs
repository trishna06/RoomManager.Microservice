using MediatR;
using Microservice.Utility.Domain.SeedWork;
using Microservice.Utility.Infrastructure;
using Microservice.Utility.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using RoomManager.Infrastructure.EntityConfigurations;

namespace RoomManager.Infrastructure
{
    public class RoomManagerContext : MicroserviceContextBase
    {
        public const string TablePrefix = "RM";
        public DbSet<Domain.Aggregates.RoomAggregate.Room> Room { get; set; }
        public DbSet<Domain.Aggregates.RoomAggregate.RoomAvailability> RoomAvailability { get; set; }

        public RoomManagerContext(DbContextOptions options, IMediator mediator,
            ICustomFieldManager cfManager, IUserService userService) : base(TablePrefix, options, mediator, cfManager, userService)
        {
        }

        protected override void ApplyEntityConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoomEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoomAvailabilityEntityTypeConfiguration());
        }
    }
}
