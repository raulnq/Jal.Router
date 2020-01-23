using Jal.Router.Interface;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{

    public class SubscriptionToPublishSubscribeChannelResource : ChannelResource
    {
        public SubscriptionToPublishSubscribeChannelResource(string subscription, string path, string connectionstring, Dictionary<string, string> properties)
            :base(path, connectionstring, properties)
        {
            Subscription = subscription;
            Rules = new List<SubscriptionToPublishSubscribeChannelRule>();
        }

        public string Subscription { get; }

        public List<SubscriptionToPublishSubscribeChannelRule> Rules { get; }
    }
}