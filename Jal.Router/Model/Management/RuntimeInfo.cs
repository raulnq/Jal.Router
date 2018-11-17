using Jal.Router.Model.Inbound;
using System.Collections.Generic;

namespace Jal.Router.Model.Management
{
    public class RuntimeInfo
    {
        public List<ListenerMetadata> ListenersMetadata { get; }

        public List<EndPoint> EndPoints { get; }

        public List<RouteMetadata> RoutesMetadata { get; }

        public List<PointToPointChannel> PointToPointChannels { get; }

        public List<PublishSubscribeChannel> PublishSubscribeChannels { get; }

        public List<SubscriptionToPublishSubscribeChannel> SubscriptionToPublishSubscribeChannels { get; }

        public RuntimeInfo()
        {
            ListenersMetadata = new List<ListenerMetadata>();

            EndPoints = new List<EndPoint>();

            RoutesMetadata = new List<RouteMetadata>();

            PointToPointChannels = new List<PointToPointChannel>();

            PublishSubscribeChannels = new List<PublishSubscribeChannel>();

            SubscriptionToPublishSubscribeChannels = new List<SubscriptionToPublishSubscribeChannel>();
        }
    }
}