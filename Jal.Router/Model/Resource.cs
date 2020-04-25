using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Resource
    {
        public string Path { get; }

        public string ConnectionString { get; private set; }

        public Dictionary<string, string> Properties { get; }

        public ChannelType ChannelType { get; }

        public string Subscription { get; }

        public List<Rule> Rules { get; }

        public Resource(ChannelType channeltype, string path, string connectionstring, Dictionary<string, string> properties, string subscription = null)
        {
            Path = path;

            Properties = properties;

            ConnectionString = connectionstring;

            ChannelType = channeltype;

            Subscription = subscription;

            Rules = new List<Rule>();
        }

        public override string ToString()
        {
            if (ChannelType == ChannelType.PointToPoint)
            {
                return "point to point";
            }
            if (ChannelType == ChannelType.PublishSubscribe)
            {
                return "publish subscribe";
            }
            if (ChannelType == ChannelType.SubscriptionToPublishSubscribe)
            {
                return "subscription to publish subscribe";
            }

            return string.Empty;
        }

        public string Id
        {
            get
            {
                return Path + Subscription + ConnectionString;
            }
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

                return description;
            }

        }
    }
}