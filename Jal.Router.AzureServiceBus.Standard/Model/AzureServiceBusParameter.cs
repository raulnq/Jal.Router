using System;

namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusParameter
    {
        public int MaxConcurrentCalls { get; set; }

        public int AutoRenewTimeoutInMinutes { get; set; }

        public int Timeout { get; set; }

        public AzureServiceBusParameter()
        {
            MaxConcurrentCalls = 4;

            AutoRenewTimeoutInMinutes = 60;
        }
    }
}