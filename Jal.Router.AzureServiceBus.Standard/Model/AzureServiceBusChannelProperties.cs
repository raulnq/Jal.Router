namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusChannelProperties
    {
        public int DefaultMessageTtlInDays { get; set; }

        public int MessageLockDurationInSeconds { get; set; }

        public int DuplicateMessageDetectionInMinutes { get; set; }

        public bool? SessionEnabled { get; set; }

        public bool? PartitioningEnabled { get; set; }

        public bool? ExpressMessageEnabled { get; set; }
    }
}
