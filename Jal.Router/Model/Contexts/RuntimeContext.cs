using Jal.Router.Model;
using Jal.Router.Model.Management;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class RuntimeContext
    {
        public List<ListenerContext> ListenerContexts { get; }

        public List<EndPoint> EndPoints { get; }

        public List<Group> Groups { get; }

        public List<Saga> Sagas { get; }

        public List<Route> Routes { get; }

        public List<SenderContext> SenderContexts { get; }

        public List<PointToPointChannel> PointToPointChannels { get; }

        public List<PublishSubscribeChannel> PublishSubscribeChannels { get; }

        public List<SubscriptionToPublishSubscribeChannel> SubscriptionToPublishSubscribeChannels { get; }

        public RuntimeContext()
        {
            ListenerContexts = new List<ListenerContext>();

            SenderContexts = new List<SenderContext>();

            EndPoints = new List<EndPoint>();

            Sagas = new List<Saga>();

            Routes = new List<Route>();

            PointToPointChannels = new List<PointToPointChannel>();

            PublishSubscribeChannels = new List<PublishSubscribeChannel>();

            SubscriptionToPublishSubscribeChannels = new List<SubscriptionToPublishSubscribeChannel>();

            Groups = new List<Group>();
        }
    }
}