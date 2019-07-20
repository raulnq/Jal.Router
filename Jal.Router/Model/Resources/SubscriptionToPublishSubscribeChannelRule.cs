﻿namespace Jal.Router.Model
{
    public class SubscriptionToPublishSubscribeChannelRule
    {
        public SubscriptionToPublishSubscribeChannelRule(string filter, string name, bool isdefault)
        {
            Filter = filter;
            Name = name;
            IsDefault = isdefault;
        }
        public string Filter { get; private set; }
        public string Name { get; private set; }
        public bool IsDefault { get; private set; }
    }

}