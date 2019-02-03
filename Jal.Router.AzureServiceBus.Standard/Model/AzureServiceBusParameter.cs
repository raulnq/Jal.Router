using System;

namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusParameter
    {
        public int MaxConcurrentCalls { get; set; }

        public int MaxConcurrentGroups { get; set; }

        public int MessageGroupTimeoutInSeconds { get; set; }

        public int AutoRenewGroupTimeoutInSeconds { get; set; }

        public int AutoRenewTimeoutInMinutes { get; set; }

        public int Timeout { get; set; }

        public AzureServiceBusParameter()
        {
            MaxConcurrentCalls = 4;

            AutoRenewTimeoutInMinutes = 60;

            MaxConcurrentGroups = 1;

            MessageGroupTimeoutInSeconds = 30;

            AutoRenewGroupTimeoutInSeconds = 30;
        }
    }
}