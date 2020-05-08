using System.Collections.Generic;

namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusChannelProperties
    {
        public const string _DefaultMessageTtlInDays = "defaultmessagettlindays";

        public const string _MessageLockDurationInSeconds = "messagelockdurationinseconds";

        public const string _DuplicateMessageDetectionInMinutes = "duplicatemessagedetectioninminutes";

        public const string _SessionEnabled = "sessionenabled";

        public const string _PartitioningEnabled = "partitioningenabled";

        public const string _ExpressMessageEnabled = "expressmessageenabled";

        public int DefaultMessageTtlInDays { get; set; }

        public int MessageLockDurationInSeconds { get; set; }

        public int DuplicateMessageDetectionInMinutes { get; set; }

        public bool? SessionEnabled { get; set; }

        public bool? PartitioningEnabled { get; set; }

        public bool? ExpressMessageEnabled { get; set; }

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
