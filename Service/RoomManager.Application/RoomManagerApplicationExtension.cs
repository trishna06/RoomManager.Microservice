using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.Utility.Events;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RoomManager.Application.Behaviours;
using RoomManager.Application.Helpers;
using RoomManager.Application.Queries;
using RoomManager.Application.BackgroundServices;

namespace RoomManager.Application
{
    public static class RoomManagerApplicationExtension
    {
        public static IServiceCollection AddRoomApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddRoomQueries();
            return services;
        }

        public static IServiceCollection AddRoomQueries(this IServiceCollection services)
        {
            services.AddHostedService<KafkaConsumerService>();
            services.AddTransient<KafkaProducerHelper>();
            services.AddTransient<IRoomQueries, RoomQueries>();
            return services;
        }
    }
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                   .AsImplementedInterfaces().InstancePerLifetimeScope();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(RoomManagerApplicationExtension).GetTypeInfo().Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<>));

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(RoomManagerApplicationExtension).GetTypeInfo().Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(typeof(RoomManagerApplicationExtension).GetTypeInfo().Assembly)
                   .AsClosedTypesOf(typeof(INotificationHandler<>));

            // Register the Command's Validators (Validators based on FluentValidation library)
            builder.RegisterAssemblyTypes(typeof(RoomManagerApplicationExtension).GetTypeInfo().Assembly)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces();

            builder.Populate(new ServiceCollection());

            builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ValidatorBehaviour<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerLifetimeScope();
        }
    }

    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(RoomManagerApplicationExtension).GetTypeInfo().Assembly)
                   .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
