using PermissionManagement.Model.Kafka;

namespace PermissionManagement.Service.Kafka
{
    public interface IKafkaService
    {
        void ProduceMessage(KafkaMessageModel message, string topic);

    }
}
