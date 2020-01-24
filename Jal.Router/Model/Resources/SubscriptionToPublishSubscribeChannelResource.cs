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
            Rules = new List<SubscriptionToPublishSubscribeChannelResourceRule>();
        }

        public string Subscription { get; }

        public List<SubscriptionToPublishSubscribeChannelResourceRule> Rules { get; }
    }
}