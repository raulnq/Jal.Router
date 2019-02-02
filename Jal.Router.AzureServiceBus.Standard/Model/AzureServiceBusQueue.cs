namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusQueue
    {
        public AzureServiceBusQueue(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public int DefaultMessageTtlInDays { get; set; }

        public int MessageLockDurationInSeconds { get; set; }

        public int DuplicateMessageDetectionInMinutes { get; set; }

        public bool? SessionEnabled { get; set; }

        public bool? PartitioningEnabled { get; set; }

        public bool? ExpressMessageEnabled { get; set; }
    }
}
