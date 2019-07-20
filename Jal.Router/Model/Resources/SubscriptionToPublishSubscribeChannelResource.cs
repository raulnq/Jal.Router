using Jal.Router.Interface;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{

    public class SubscriptionToPublishSubscribeChannelResource
    {
        public SubscriptionToPublishSubscribeChannelResource(string subscription, string path, string connectionstring, Dictionary<string, string> properties)
        {
            Subscription = subscription;
            Path = path;
            Rules = new List<SubscriptionToPublishSubscribeChannelRule>();
            ConnectionString = connectionstring;
            Properties = properties;
        }

        public SubscriptionToPublishSubscribeChannelResource(string subscription, string path, Dictionary<string, string> properties, Type type, Func<IValueFinder, string> provider)
        {
            Subscription = subscription;
            Path = path;
            Rules = new List<SubscriptionToPublishSubscribeChannelRule>();
            Properties = properties;
            ConnectionStringValueFinderType = type;
            ConnectionStringProvider = provider;
        }

        public string Subscription { get; }

        public Type ConnectionStringValueFinderType { get; }

        public Func<IValueFinder, string> ConnectionStringProvider { get; }

        public string Path { get; }

        public string ConnectionString { get; private set; }

        public List<SubscriptionToPublishSubscribeChannelRule> Rules { get; }

        public Dictionary<string, string> Properties { get; }

        public void UpdateConnectionString(string connectionstring)
        {
            ConnectionString = connectionstring;
        }
    }
}