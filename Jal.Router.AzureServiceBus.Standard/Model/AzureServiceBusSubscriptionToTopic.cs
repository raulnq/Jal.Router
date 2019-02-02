namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class AzureServiceBusSubscriptionToTopic
    {
        public AzureServiceBusSubscriptionToTopic(string name, string topic)
        {
            Name = name;
            Topic = topic;
        }
        public string Name { get; set; }

        public string Topic { get; set; }

        public bool? SessionEnabled { get; set; }

        public int DefaultMessageTtlInDays { get; set; }

        public int MessageLockDurationInSeconds { get; set; }

    }
}
