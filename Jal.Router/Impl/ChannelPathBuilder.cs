using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ChannelPathBuilder : IChannelPathBuilder
    {
        public string BuildFromSagaAndRoute(Saga saga, string routeName, Channel channel)
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
        public string BuildFromRoute(string name, Channel channel)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(name))
            {
                channelpath = $"{channelpath}/{name}";
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
        public string BuildFromEndpoint(string name, Channel channel)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(name))
            {
                channelpath = $"{channelpath}/{name}";
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
        public string BuildReplyFromEndpoint(string name, Channel channel)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(name))
            {
                channelpath = $"{channelpath}/{name}";
            }

            if (!string.IsNullOrWhiteSpace(channel.ToReplyPath))
            {
                channelpath = $"{channelpath}/{channel.ToReplyPath}";
            }

            if (!string.IsNullOrWhiteSpace(channel.ToReplySubscription))
            {
                channelpath = $"{channelpath}/{channel.ToReplySubscription}";
            }

            return channelpath;
        }
    }
}