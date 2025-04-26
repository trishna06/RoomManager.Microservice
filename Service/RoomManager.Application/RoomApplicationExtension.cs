using System.Reflection;
using Autofac;
using RoomManager.Application.Queries;
using EventBusUtility.Events;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RoomManager.Application.BackgroundServices;
using RoomManager.Application.Behaviours;

namespace RoomManager.Application
{
    public static class RoomApplicationExtension
    {
        public static IServiceCollection AddRoomApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddRoomQueries();
            return services;
        }

        public static IServiceCollection AddRoomQueries(this IServiceCollection services)
        {
            //services.AddHostedService<KafkaConsumerService>();
            services.AddTransient<IRoomQueries, RoomQueries>();
            return services;
        }
    }

    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                   .AsImplementedInterfaces();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(RoomApplicationExtension).GetTypeInfo().Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(typeof(RoomApplicationExtension).GetTypeInfo().Assembly)
                   .AsClosedTypesOf(typeof(INotificationHandler<>));

            // Register the Command's Validators (Validators based on FluentValidation library)            
            builder.RegisterAssemblyTypes(typeof(RoomApplicationExtension).GetTypeInfo().Assembly)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                IComponentContext componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }

    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(RoomApplicationExtension).GetTypeInfo().Assembly)
                   .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
