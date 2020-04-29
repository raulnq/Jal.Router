using System.Text;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ChannelValidator : IChannelValidator
    {
        private ILogger _logger;

        public ChannelValidator(ILogger logger)
        {
            _logger = logger;
        }

        public string Validate(Channel channel, string channeltype, string channelname)
        {
            StringBuilder errors = new StringBuilder();

            Validate(errors, channel.ConnectionString, "connection string", channeltype, channelname);

            Validate(errors, channel.Path, "path", channeltype, channelname);

            if (channel.ChannelType == ChannelType.SubscriptionToPublishSubscribe)
            {
                Validate(errors, channel.Subscription, "subscription", channeltype, channelname);
            }

            if (channel.ReplyType != ReplyType.None)
            {
                Validate(errors, channel.ReplyConnectionString, "reply connection string", channeltype, channelname);

                Validate(errors, channel.ReplyPath, "reply path", channeltype, channelname);

                if (channel.ReplyType == ReplyType.FromSubscriptionToPublishSubscribe)
                {
                    Validate(errors, channel.ReplySubscription, "reply subscription", channeltype, channelname);
                }
            }

            return errors.ToString();
        }

        private void Validate(StringBuilder errors, string value, string propertyname, string channeltype, string channelname)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                var error = $"Empty {propertyname}, {channeltype} {channelname}";

                errors.AppendLine(error);

                _logger.Log(error);
            }
        }
    }
}