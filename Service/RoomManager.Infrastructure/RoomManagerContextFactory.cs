using System;
using Microservice.Utility.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RoomManager.Infrastructure
{
    public class RoomManagerContextFactory : IDesignTimeDbContextFactory<RoomManagerContext>
    {
        public RoomManagerContext CreateDbContext(string[] args)
        {
            string connectionString = $"Server=localhost;Database=UEIOS_V4_DEV;User Id=arcstone;Password=Juniormints123;";

            DbContextOptionsBuilder<RoomManagerContext> optionsBuilder = new DbContextOptionsBuilder<RoomManagerContext>();
            optionsBuilder.UseSqlServer(connectionString, providerOptions =>
            {
                providerOptions.CommandTimeout(30);
                providerOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
                providerOptions.MigrationsHistoryTable($"{RoomManagerContext.TablePrefix}_EF_MIGRATIONS_HISTORY".ApplyConvention());
            });

            return new RoomManagerContext(optionsBuilder.Options, null, null, null);
        }
    }
}
