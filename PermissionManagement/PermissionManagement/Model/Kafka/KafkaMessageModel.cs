using PermissionManagement.Helpers;

namespace PermissionManagement.Model.Kafka
{
    public class KafkaMessageModel
    {
        public Guid Id { get; set; }
        public string Operation { get; set; }
        public DateTime CreatedDate { get; set; }

        public KafkaMessageModel(EnumConstants.enumOperation operation)
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            switch (operation)
            {
                case EnumConstants.enumOperation.modify:
                    {
                        Operation = "Modify";
                        break;
                    }
                case EnumConstants.enumOperation.request:
                    {
                        Operation = "Request";
                        break;
                    }
                case EnumConstants.enumOperation.get:
                    {
                        Operation = "Get";
                        break;
                    }
                default:
                    {
                        Operation = "Get";
                        break;
                    }
            }
        }
    }
}
