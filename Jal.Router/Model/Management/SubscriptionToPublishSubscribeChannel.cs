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
            Properties = new Dictionary<string, string>();
        }

        public string Subscription { get; }

        public Type ConnectionStringValueFinderType { get; set; }

        public object ConnectionStringProvider { get; set; }

        public string Path { get; }

        public string ConnectionString { get; set; }

        public List<SubscriptionToPublishSubscribeChannelRule> Rules { get; }

        public Dictionary<string, string> Properties { get; set; }
    }
}