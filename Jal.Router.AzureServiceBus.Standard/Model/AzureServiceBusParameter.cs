using System;

namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusParameter
    {
        public int MaxConcurrentCalls { get; set; }

        public int MaxConcurrentPartitions { get; set; }

        public int MessagePartitionTimeoutInSeconds { get; set; }

        public int AutoRenewPartitionTimeoutInSeconds { get; set; }

        public int AutoRenewTimeoutInMinutes { get; set; }

        public int TimeoutInSeconds { get; set; }

        public AzureServiceBusParameter()
        {
            MaxConcurrentCalls = 4;

            AutoRenewTimeoutInMinutes = 60;

            MaxConcurrentPartitions = 1;

            MessagePartitionTimeoutInSeconds = 30;

            AutoRenewPartitionTimeoutInSeconds = 30;

            TimeoutInSeconds = 0;
        }
    }
}