using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoomManager.Application.Commands.DataTransferObjects;

namespace RoomManager.Application.BackgroundServices
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;

        public KafkaConsumerService(IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _config = config;
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

            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<string, string> result = consumer.Consume(stoppingToken);
                RoomAvailabilityDto update = JsonSerializer.Deserialize<RoomAvailabilityDto>(result.Message.Value);

                // update db status
            }
        }
    }

}
