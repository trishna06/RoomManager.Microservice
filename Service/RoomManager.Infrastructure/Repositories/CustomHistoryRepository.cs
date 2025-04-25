using System.Diagnostics.CodeAnalysis;
using Microservice.Utility.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace RoomManager.Infrastructure.Repositories
{
    /// <summary>
    /// Do not remove. This class is needed for EF Migration History Table to have columns in correct naming convention.
    /// </summary>
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "Inherit MSSQL default implmentation as-is")]
    [ExcludeFromCodeCoverage]
    internal sealed class CustomHistoryRepository : SqlServerHistoryRepository
    {
        public CustomHistoryRepository(HistoryRepositoryDependencies dependencies) : base(dependencies)
        {
        }

        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);

            history.Property(h => h.MigrationId).HasColumnName(nameof(HistoryRow.MigrationId).ApplyConvention());
            history.Property(h => h.ProductVersion).HasColumnName(nameof(HistoryRow.ProductVersion).ApplyConvention());
        }
    }
}
