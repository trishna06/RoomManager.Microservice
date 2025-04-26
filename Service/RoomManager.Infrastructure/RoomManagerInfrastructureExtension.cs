using System;
using Microservice.Utility.Domain.SeedWork;
using Microservice.Utility.Infrastructure.Extensions;
using Microservice.Utility.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using RoomManager.Domain.Repositories;
using RoomManager.Infrastructure.Queries;
using RoomManager.Infrastructure.Repositories;

namespace RoomManager.Infrastructure
{
    public static class RoomManagerInfrastructureExtension
    {
        public static IServiceCollection AddRoomInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RoomManagerContext>(options =>
            {
                options.UseSqlServer(connectionString, providerOptions =>
                {
                    providerOptions.CommandTimeout(30);
                    providerOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    providerOptions.MigrationsHistoryTable($"{RoomManagerContext.TablePrefix}_EF_MIGRATIONS_HISTORY".ApplyConvention());
                }).ReplaceService<IHistoryRepository, CustomHistoryRepository>();
            }, ServiceLifetime.Scoped);

            services.AddRoomRepositories()
                    .AddCustomFieldServices();

            return services;
        }

        public static IServiceCollection AddRoomRepositories(this IServiceCollection services)
        {
            services.AddScoped<IContextQuery, RoomManagerContextQuery>();
            services.AddScoped<IRoomRepository, RoomRepository>();

            return services;
        }

        public static IServiceCollection AddCustomFieldServices(this IServiceCollection services)
        {
            services.AddSingleton<ICustomFieldManager, CustomFieldManager>();

            return services;
        }
    }
}

