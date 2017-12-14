namespace Jal.Router.Model.Management
{
    public class SubscriptionToPublishSubscribeChannelInfo
    {
        public SubscriptionToPublishSubscribeChannelInfo(string subscription, string path)
        {
            Subscription = subscription;
            Path = path;
        }

        public string Subscription { get; set; }

        public string Path { get; set; }

        public long MessageCount { get; set; }

        public long ScheduledMessageCount { get; set; }

        public long DeadLetterMessageCount { get; set; }
    }
}