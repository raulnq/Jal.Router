using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ChannelPathBuilder : IChannelPathBuilder
    {
        public string BuildFromSagaAndRoute(Saga saga, Route route)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(saga.Name))
            {
                channelpath = $"{channelpath}/{saga.Name}";
            }

            if (!string.IsNullOrWhiteSpace(route.Name))
            {
                channelpath = $"{channelpath}/{route.Name}";
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
        public string BuildFromRoute(Route route)
        {
            var channelpath = string.Empty;

            if (!string.IsNullOrWhiteSpace(route.Name))
            {
                channelpath = $"{channelpath}/{route.Name}";
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