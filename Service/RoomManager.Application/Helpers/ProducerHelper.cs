using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using RoomManager.Application.Commands.DataTransferObjects;

namespace RoomManager.Application.Helpers
{
    public class KafkaProducerHelper
    {
        private readonly IConfiguration _config;
        private readonly string _topicName = "booking.events";

        public KafkaProducerHelper(IConfiguration config)
        {
            _config = config;
        }

        public async Task ProduceAsync(RoomAvailabilityDto roomAvailability)
        {
            ProducerConfig conf = new ProducerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                SecurityProtocol = SecurityProtocol.Plaintext
            };

            using IProducer<string, string> producer = new ProducerBuilder<string, string>(conf).Build();

            Message<string, string> message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(), // Optional: generate unique key
                Value = JsonSerializer.Serialize(roomAvailability)
            };

            try
            {
                DeliveryResult<string, string> deliveryResult = await producer.ProduceAsync(_topicName, message);
                Console.WriteLine($"Delivered message to {deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
            }
        }
    }
}
