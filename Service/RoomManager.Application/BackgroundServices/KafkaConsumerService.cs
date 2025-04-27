using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoomManager.Application.Commands.DataTransferObjects;
using RoomManager.Domain.Aggregates.RoomAggregate;
using RoomManager.Domain.Repositories;

namespace RoomManager.Application.BackgroundServices
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;
        private readonly IRoomRepository _repository;

        public KafkaConsumerService(IServiceScopeFactory scopeFactory, IConfiguration config, IRoomRepository repository)
        {
            _scopeFactory = scopeFactory;
            _config = config;
            _repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsumerConfig conf = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = "room-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.Plaintext
            };

            using IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(conf).Build();
            consumer.Subscribe(new[] { "booking.events" });

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        ConsumeResult<string, string> result = consumer.Consume(TimeSpan.FromMilliseconds(1000)); // <-- Wait max 1 sec

                        if (result == null)
                        {
                            await Task.Delay(100, stoppingToken); // Wait 100ms if no event
                            continue;
                        }

                        if (result != null && result.Message != null)
                        {
                            RoomAvailabilityDto update = JsonSerializer.Deserialize<RoomAvailabilityDto>(result.Message.Value);
                            Console.WriteLine(result.Message.Value);
                            if (update != null)
                            {
                                Room room = await _repository.GetAsync(update.RoomId);
                                room.UpdateAvailability(update.Type, update.Status);
                                await _repository.UnitOfWork.SaveEntitiesAsync();
                            }
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        if (ex.Error.IsFatal)
                        {
                            // Fatal errors (like missing topic): break loop
                            Console.WriteLine($"Fatal error: {ex.Error.Reason}");
                            break;
                        }
                        else
                        {
                            // Non-fatal errors: log and continue
                            Console.WriteLine($"Consume error: {ex.Error.Reason}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unexpected error: {ex.Message}");
                    }
                }
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
