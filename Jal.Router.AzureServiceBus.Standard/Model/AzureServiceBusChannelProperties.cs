using System;
using System.Collections.Generic;

namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusChannelProperties
    {
        public const string _DefaultMessageTtlInDays = "properties_defaultmessagettlindays";

        public const string _MessageLockDurationInSeconds = "properties_messagelockdurationinseconds";

        public const string _DuplicateMessageDetectionInMinutes = "properties_duplicatemessagedetectioninminutes";

        public const string _SessionEnabled = "properties_sessionenabled";

        public const string _PartitioningEnabled = "properties_partitioningenabled";

        public const string _ExpressMessageEnabled = "properties_expressmessageenabled";

        public int DefaultMessageTtlInDays { get; set; }

        public int MessageLockDurationInSeconds { get; set; }

        public int DuplicateMessageDetectionInMinutes { get; set; }

        public bool? SessionEnabled { get; set; }

        public bool? PartitioningEnabled { get; set; }

        public bool? ExpressMessageEnabled { get; set; }

        public AzureServiceBusChannelProperties(IDictionary<string, object> properties):this()
        {
            if(properties!=null)
            {
                if (properties.ContainsKey(_DefaultMessageTtlInDays))
                {
                    DefaultMessageTtlInDays = Convert.ToInt32(properties[_DefaultMessageTtlInDays]);
                }

                if (properties.ContainsKey(_MessageLockDurationInSeconds))
                {
                    MessageLockDurationInSeconds = Convert.ToInt32(properties[_MessageLockDurationInSeconds]);
                }

                if (properties.ContainsKey(_SessionEnabled))
                {
                    SessionEnabled = true;
                }

                if (properties.ContainsKey(_DuplicateMessageDetectionInMinutes))
                {
                    DuplicateMessageDetectionInMinutes = Convert.ToInt32(properties[_DuplicateMessageDetectionInMinutes]);
                }

                if (properties.ContainsKey(_PartitioningEnabled))
                {
                    PartitioningEnabled = true;
                }

                if (properties.ContainsKey(_ExpressMessageEnabled))
                {
                    ExpressMessageEnabled = true;
                }
            }
        }

        public AzureServiceBusChannelProperties()
        {
            DefaultMessageTtlInDays = 14;

            MessageLockDurationInSeconds = 300;
        }

        public IDictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            if (DefaultMessageTtlInDays > 0)
            {
                dictionary.Add(_DefaultMessageTtlInDays, DefaultMessageTtlInDays.ToString());
            }
            if (DuplicateMessageDetectionInMinutes > 0)
            {
                dictionary.Add(_DuplicateMessageDetectionInMinutes, DuplicateMessageDetectionInMinutes.ToString());
            }
            if (ExpressMessageEnabled != null)
            {
                dictionary.Add(_ExpressMessageEnabled, "true");
            }
            if (MessageLockDurationInSeconds > 0)
            {
                dictionary.Add(_MessageLockDurationInSeconds, MessageLockDurationInSeconds.ToString());
            }
            if (PartitioningEnabled != null)
            {
                dictionary.Add(_PartitioningEnabled, "true");
            }
            if (SessionEnabled != null)
            {
                dictionary.Add(_SessionEnabled, "true");
            }

            return dictionary;
        }
    }
}
