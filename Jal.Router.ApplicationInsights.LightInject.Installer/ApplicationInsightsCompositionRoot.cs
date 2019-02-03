using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Jal.Router.Model.Management;
using LightInject;

namespace Jal.Router.ApplicationInsights.LightInject.Installer
{
    public class ApplicationInsightsCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ILogger<PointToPointChannelInfo>, PointToPointChannelInfoLogger>(typeof(PointToPointChannelInfoLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<Beat>, BeatLogger>(typeof(BeatLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<SubscriptionToPublishSubscribeChannelInfo>, SubscriptionToPublishSubscribeChannelInfoLogger>(typeof(SubscriptionToPublishSubscribeChannelInfoLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<PublishSubscribeChannelInfo>, PublishSubscribeChannelInfoLogger>(typeof(PublishSubscribeChannelInfoLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IMiddleware<MessageContext>, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IMiddleware<MessageContext>, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());
        }
    }
}
