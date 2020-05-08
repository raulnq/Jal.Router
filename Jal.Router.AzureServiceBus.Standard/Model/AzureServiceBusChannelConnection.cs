using System;
using System.Collections.Generic;

namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusChannelConnection
    {
        public int MaxConcurrentCalls { get; set; }

        public int MaxConcurrentPartitions { get; set; }

        public int MessagePartitionTimeoutInSeconds { get; set; }

        public int AutoRenewPartitionTimeoutInSeconds { get; set; }

        public int AutoRenewTimeoutInMinutes { get; set; }

        public int TimeoutInSeconds { get; set; }

        public AzureServiceBusChannelConnection()
        {
            MaxConcurrentCalls = 4;

            AutoRenewTimeoutInMinutes = 60;

            MaxConcurrentPartitions = 1;

            MessagePartitionTimeoutInSeconds = 30;

            AutoRenewPartitionTimeoutInSeconds = 30;

            TimeoutInSeconds = 0;
        }

        public const string _MaxConcurrentCalls = "maxconcurrentcalls";

        public const string _MaxConcurrentPartitions = "maxconcurrentpartitions";

        public const string _AutoRenewPartitionTimeoutInSeconds = "autorenewpartitiontimeoutinseconds";

        public const string _AutoRenewTimeoutInMinutes = "autorenewtimeoutinminutes";

        public const string _MessagePartitionTimeoutInSeconds = "messagepartitiontimeoutinseconds";

        public const string _TimeoutInSeconds = "timeoutinseconds";

        public IDictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            if (MaxConcurrentCalls > 0)
            {
                dictionary.Add(_MaxConcurrentCalls, MaxConcurrentCalls.ToString());
            }
            if (MaxConcurrentPartitions > 0)
            {
                dictionary.Add(_MaxConcurrentPartitions, MaxConcurrentPartitions.ToString());
            }
            if (AutoRenewPartitionTimeoutInSeconds > 0)
            {
                dictionary.Add(_AutoRenewPartitionTimeoutInSeconds, AutoRenewPartitionTimeoutInSeconds.ToString());
            }
            if (AutoRenewTimeoutInMinutes > 0)
            {
                dictionary.Add(_AutoRenewTimeoutInMinutes, AutoRenewTimeoutInMinutes.ToString());
            }
            if (MessagePartitionTimeoutInSeconds > 0)
            {
                dictionary.Add(_MessagePartitionTimeoutInSeconds, MessagePartitionTimeoutInSeconds.ToString());
            }
            if (TimeoutInSeconds > 0)
            {
                dictionary.Add(_TimeoutInSeconds, TimeoutInSeconds.ToString());
            }

            return dictionary;
        }
    }
}