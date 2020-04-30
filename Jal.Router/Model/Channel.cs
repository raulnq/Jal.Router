using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Channel
    {
        public Channel(ChannelType channeltype, string connectionstring, string path, Type adaptertype, Type type, bool fromrouterconfigurationsource = true)
        {
            ChannelType = channeltype;

            ConnectionString = connectionstring;

            Path = path;

            RouterConfigurationSource = fromrouterconfigurationsource;

            AdapterType = adaptertype;

            Type = type;

            Id = Guid.NewGuid().ToString();

            Properties = new Dictionary<string, object>();

            Rules = new List<Rule>();
        }

        public Channel(ChannelType channeltype, string connectionstring, string path, string subscription, Type adaptertype, Type type, bool fromrouterconfigurationsource = true)
            :this(channeltype, connectionstring, path, adaptertype, type, fromrouterconfigurationsource)
        {
            Subscription = subscription;
        }

        public IDictionary<string, object> Properties { get; }

        public IList<Rule> Rules { get; }

        public Func<MessageContext, bool> Condition { get; private set; }

        public bool UseClaimCheck { get; private set; }

        public bool UseCreateIfNotExists { get; private set; }

        public bool UsePartition { get; private set; }

        public Func<MessageContext, bool> ClosePartitionCondition { get; private set; }

        public bool RouterConfigurationSource { get; private set; }

        public Type AdapterType { get; set; }

        public Type Type { get; set; }

        public ChannelType ChannelType { get; private set; }

        public Channel ReplyChannel { get; private set; }

        public string Id { get; }

        public override string ToString()
        {
            var type = string.Empty;

            if (ChannelType == ChannelType.PointToPoint)
            {
                type = "point to point";
            }
            if (ChannelType == ChannelType.PublishSubscribe)
            {
                type =  "publish subscribe";
            }
            if (ChannelType == ChannelType.SubscriptionToPublishSubscribe)
            {
                type = "subscription to publish subscribe";
            }

            if(ReplyChannel!=null)
            {
                if (ReplyChannel.ChannelType == ChannelType.PointToPoint)
                {
                    type = type + " (reply from point to point channel)";
                }

                if (ReplyChannel.ChannelType == ChannelType.SubscriptionToPublishSubscribe)
                {
                    type = type + " (reply from subscription to publish subscribe channel)";
                }
            }

            return type;
        }

        public string FullPath
        {
            get
                {
                var description = string.Empty;

                if (!string.IsNullOrWhiteSpace(Path))
                {
                    description = $"{description}/{Path}";
                }

                if (!string.IsNullOrWhiteSpace(Subscription))
                {
                    description = $"{description}/{Subscription}";
                }

                if (ReplyChannel!=null)
                {
                    description = $"{description} - {ReplyChannel.FullPath}"; 
                }

                return description;
            }
        }

        public string ConnectionString { get; private set; }

        public string Path { get; private set; }

        public string Subscription { get; private set; }

        public int ReplyTimeOut { get; private set; }

        public ChannelEntity ToEntity()
        {
            return new ChannelEntity(Path, Subscription, ChannelType);
        }

        public void AsClaimCheck()
        {
            UseClaimCheck = true;
        }

        public void CreateIfNotExists()
        {
            UseCreateIfNotExists = true;
        }

        public void Partition(Func<MessageContext, bool> condition=null)
        {
            UsePartition = true;
            ClosePartitionCondition = condition;
        }

        public void When(Func<MessageContext, bool> when)
        {
            Condition = when;
        }


        public void ReplyTo(Channel channel, int replytimeout)
        {
            ReplyChannel = channel;
            ReplyTimeOut = replytimeout;
        }
    }
}