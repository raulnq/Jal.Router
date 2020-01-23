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

        public List<PointToPointChannelResource> PointToPointChannelResources { get; }

        public List<PublishSubscribeChannelResource> PublishSubscribeChannelResources { get; }

        public List<SubscriptionToPublishSubscribeChannelResource> SubscriptionToPublishSubscribeChannelResources { get; }

        public RuntimeContext()
        {
            ListenerContexts = new List<ListenerContext>();

            SenderContexts = new List<SenderContext>();

            EndPoints = new List<EndPoint>();

            Sagas = new List<Saga>();

            Routes = new List<Route>();

            PointToPointChannelResources = new List<PointToPointChannelResource>();

            PublishSubscribeChannelResources = new List<PublishSubscribeChannelResource>();

            SubscriptionToPublishSubscribeChannelResources = new List<SubscriptionToPublishSubscribeChannelResource>();

            Partitions = new List<Partition>();
        }
    }
}