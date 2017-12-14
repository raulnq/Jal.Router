using System;

namespace Jal.Router.Model.Management
{
    public class SubscriptionToPublishSubscribeChannel
    {
        public SubscriptionToPublishSubscribeChannel(string subscription, string path)
        {
            Subscription = subscription;
            Path = path;
        }
        public Origin Origin { get; set; }

        public string Subscription { get; set; }

        public Type PathExtractorType { get; set; }

        public Type ConnectionStringExtractorType { get; set; }

        public object ToConnectionStringExtractor { get; set; }

        public string Path { get; set; }
    }
}