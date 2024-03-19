using Confluent.Kafka;
using PermissionManagement.Model.Kafka;
using System.Text.Json;

namespace PermissionManagement.Service.Kafka
{
    public class KafkaService : IKafkaService
    {
        private readonly IProducer<string, string> _kafkaProducer;

        public KafkaService(IProducer<string, string> kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        public void ProduceMessage(KafkaMessageModel message, string topic)
        {
            _kafkaProducer.Produce(topic, new Message<string, string> { Value = JsonSerializer.Serialize(message) });
        }
    }
}
