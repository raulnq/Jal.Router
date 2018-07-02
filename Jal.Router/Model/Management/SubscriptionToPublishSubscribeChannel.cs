using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Management
{

    public class SubscriptionToPublishSubscribeChannel
    {
        public SubscriptionToPublishSubscribeChannel(string subscription, string path)
        {
            Subscription = subscription;
            Path = path;
            Rules = new List<SubscriptionToPublishSubscribeChannelRule>();
        }

        public string Subscription { get; set; }

        public Type ConnectionStringExtractorType { get; set; }

        public object ConnectionStringExtractor { get; set; }

        public string Path { get; set; }

        public string ConnectionString { get; set; }

        public List<SubscriptionToPublishSubscribeChannelRule> Rules { get; set; }
    }
}