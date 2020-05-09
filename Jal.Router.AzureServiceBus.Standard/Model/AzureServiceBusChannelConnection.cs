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

        public bool LogClientException { get; set; }

        public AzureServiceBusChannelConnection()
        {
            MaxConcurrentCalls = 4;

            AutoRenewTimeoutInMinutes = 60;

            MaxConcurrentPartitions = 1;

            MessagePartitionTimeoutInSeconds = 30;

            AutoRenewPartitionTimeoutInSeconds = 30;

            TimeoutInSeconds = 0;

            LogClientException = false;
        }

        public AzureServiceBusChannelConnection(IDictionary<string, object> properties):this()
        {
            if(properties!=null)
            {
                if (properties.ContainsKey(_MaxConcurrentCalls))
                {
                    MaxConcurrentCalls = Convert.ToInt32(properties.ContainsKey(_MaxConcurrentCalls).ToString());
                }
                if (properties.ContainsKey(_MaxConcurrentPartitions))
                {
                    MaxConcurrentPartitions = Convert.ToInt32(properties.ContainsKey(_MaxConcurrentPartitions).ToString());
                }
                if (properties.ContainsKey(_AutoRenewPartitionTimeoutInSeconds))
                {
                    AutoRenewPartitionTimeoutInSeconds = Convert.ToInt32(properties.ContainsKey(_AutoRenewPartitionTimeoutInSeconds).ToString());
                }
                if (properties.ContainsKey(_AutoRenewTimeoutInMinutes))
                {
                    AutoRenewTimeoutInMinutes = Convert.ToInt32(properties.ContainsKey(_AutoRenewTimeoutInMinutes).ToString());
                }
                if (properties.ContainsKey(_MessagePartitionTimeoutInSeconds))
                {
                    MessagePartitionTimeoutInSeconds = Convert.ToInt32(properties.ContainsKey(_MessagePartitionTimeoutInSeconds).ToString());
                }
                if (properties.ContainsKey(_TimeoutInSeconds))
                {
                    TimeoutInSeconds = Convert.ToInt32(properties.ContainsKey(_TimeoutInSeconds).ToString());
                }
                LogClientException = Convert.ToBoolean(properties.ContainsKey(_LogClientException).ToString());
            }

        }

        public const string _MaxConcurrentCalls = "connection_maxconcurrentcalls";

        public const string _MaxConcurrentPartitions = "connection_maxconcurrentpartitions";

        public const string _AutoRenewPartitionTimeoutInSeconds = "connection_autorenewpartitiontimeoutinseconds";

        public const string _AutoRenewTimeoutInMinutes = "connection_autorenewtimeoutinminutes";

        public const string _MessagePartitionTimeoutInSeconds = "connection_messagepartitiontimeoutinseconds";

        public const string _TimeoutInSeconds = "connection_timeoutinseconds";

        public const string _LogClientException = "connection_logclientexception";

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

            dictionary.Add(_LogClientException, LogClientException.ToString());

            return dictionary;
        }
    }
}