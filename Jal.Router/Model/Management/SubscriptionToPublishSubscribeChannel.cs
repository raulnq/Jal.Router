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

        public string Subscription { get; }

        public Type ConnectionStringValueFinderType { get; set; }

        public object ConnectionStringProvider { get; set; }

        public string Path { get; }

        public string ConnectionString { get; set; }

        public List<SubscriptionToPublishSubscribeChannelRule> Rules { get; }
    }
}