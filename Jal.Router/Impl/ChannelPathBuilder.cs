using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ChannelPathBuilder : IChannelPathBuilder
    {
        public string BuildFromSagaAndRoute(Saga saga, string routeName, Channel route)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(saga.Name))
            {
                channelpath = $"{channelpath}/{saga.Name}";
            }

            if (!string.IsNullOrWhiteSpace(routeName))
            {
                channelpath = $"{channelpath}/{routeName}";
            }

            if (!string.IsNullOrWhiteSpace(route.ToPath))
            {
                channelpath = $"{channelpath}/{route.ToPath}";
            }

            if (!string.IsNullOrWhiteSpace(route.ToSubscription))
            {
                channelpath = $"{channelpath}/{route.ToSubscription}";
            }
            return channelpath;
        }
        public string BuildFromRoute(string routeName, Channel channel)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(routeName))
            {
                channelpath = $"{channelpath}/{routeName}";
            }

            if (!string.IsNullOrWhiteSpace(channel.ToPath))
            {
                channelpath = $"{channelpath}/{channel.ToPath}";
            }

            if (!string.IsNullOrWhiteSpace(channel.ToSubscription))
            {
                channelpath = $"{channelpath}/{channel.ToSubscription}";
            }

            return channelpath;
        }
        public string BuildFromContext(MessageContext context)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(context.EndPointName))
            {
                channelpath = $"{channelpath}/{context.EndPointName}";
            }

            if (!string.IsNullOrWhiteSpace(context.ToPath))
            {
                channelpath = $"{channelpath}/{context.ToPath}";
            }

            if (!string.IsNullOrWhiteSpace(context.ToSubscription))
            {
                channelpath = $"{channelpath}/{context.ToSubscription}";
            }

            return channelpath;
        }

        public string BuildReplyFromContext(MessageContext context)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(context.EndPointName))
            {
                channelpath = $"{channelpath}/{context.EndPointName}";
            }

            if (!string.IsNullOrWhiteSpace(context.ToReplyPath))
            {
                channelpath = $"{channelpath}/{context.ToReplyPath}";
            }

            if (!string.IsNullOrWhiteSpace(context.ToReplySubscription))
            {
                channelpath = $"{channelpath}/{context.ToReplySubscription}";
            }

            return channelpath;
        }

    }
}