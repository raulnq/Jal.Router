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

        public string Validate(Channel channel, string resourcetype, string resourcename)
        {
            StringBuilder errors = new StringBuilder();

            Validate(errors, channel.ConnectionString, "connection string", resourcetype, resourcename);

            Validate(errors, channel.Path, "path", resourcetype, resourcename);

            if (channel.Type == ChannelType.SubscriptionToPublishSubscribe)
            {
                Validate(errors, channel.Subscription, "subscription", resourcetype, resourcename);
            }

            if (channel.Type == ChannelType.RequestReplyToPointToPoint || channel.Type == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
            {
                Validate(errors, channel.ReplyConnectionString, "reply connection string", resourcetype, resourcename);

                Validate(errors, channel.ReplyPath, "reply path", resourcetype, resourcename);

                if (channel.Type == Model.ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
                {
                    Validate(errors, channel.ReplySubscription, "reply subscription", resourcetype, resourcename);
                }
            }

            return errors.ToString();
        }

        private void Validate(StringBuilder errors, string value, string propertyname, string resourcetype, string resourcename)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                var error = $"Empty {propertyname}, {resourcetype} {resourcename}";

                errors.AppendLine(error);

                _logger.Log(error);
            }
        }
    }
}