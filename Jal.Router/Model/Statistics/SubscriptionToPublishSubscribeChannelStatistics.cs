using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class SubscriptionToPublishSubscribeChannelStatistics
    {
        public SubscriptionToPublishSubscribeChannelStatistics(string subscription, string path)
        {
            Subscription = subscription;
            Path = path;
            Properties = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Properties { get; }

        public string Subscription { get; }

        public string Path { get; }
    }
}