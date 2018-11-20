using Jal.Router.Model.Inbound;
using System.Collections.Generic;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Model.Management
{
    public class Runtime
    {
        public List<ListenerMetadata> ListenersMetadata { get; }

        public List<EndPoint> EndPoints { get; }

        public List<Saga> Sagas { get; }

        public List<Route> Routes { get; }

        public List<SenderMetadata> SendersMetadata { get; }

        public List<PointToPointChannel> PointToPointChannels { get; }

        public List<PublishSubscribeChannel> PublishSubscribeChannels { get; }

        public List<SubscriptionToPublishSubscribeChannel> SubscriptionToPublishSubscribeChannels { get; }

        public Runtime()
        {
            ListenersMetadata = new List<ListenerMetadata>();

            SendersMetadata = new List<SenderMetadata>();

            EndPoints = new List<EndPoint>();

            Sagas = new List<Saga>();

            Routes = new List<Route>();

            PointToPointChannels = new List<PointToPointChannel>();

            PublishSubscribeChannels = new List<PublishSubscribeChannel>();

            SubscriptionToPublishSubscribeChannels = new List<SubscriptionToPublishSubscribeChannel>();
        }
    }
}