using Jal.Router.Model;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class RuntimeContext
    {
        public List<ListenerContext> ListenerContexts { get; }

        public List<EndPoint> EndPoints { get; }

        public List<Partition> Partitions { get; }

        public List<Saga> Sagas { get; }

        public List<Route> Routes { get; }

        public List<SenderContext> SenderContexts { get; }

        public List<PointToPointChannelResource> PointToPointChannels { get; }

        public List<PublishSubscribeChannelResource> PublishSubscribeChannels { get; }

        public List<SubscriptionToPublishSubscribeChannelResource> SubscriptionToPublishSubscribeChannels { get; }

        public RuntimeContext()
        {
            ListenerContexts = new List<ListenerContext>();

            SenderContexts = new List<SenderContext>();

            EndPoints = new List<EndPoint>();

            Sagas = new List<Saga>();

            Routes = new List<Route>();

            PointToPointChannels = new List<PointToPointChannelResource>();

            PublishSubscribeChannels = new List<PublishSubscribeChannelResource>();

            SubscriptionToPublishSubscribeChannels = new List<SubscriptionToPublishSubscribeChannelResource>();

            Partitions = new List<Partition>();
        }
    }
}