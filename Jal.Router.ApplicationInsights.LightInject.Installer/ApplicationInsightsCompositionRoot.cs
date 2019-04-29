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
            serviceRegistry.Register<ILogger<PointToPointChannelStatistics>, PointToPointChannelStatisticsLogger>(typeof(PointToPointChannelStatisticsLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<Beat>, BeatLogger>(typeof(BeatLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<SubscriptionToPublishSubscribeChannelStatistics>, SubscriptionToPublishSubscribeChannelStatisticsLogger>(typeof(SubscriptionToPublishSubscribeChannelStatisticsLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<PublishSubscribeChannelStatistics>, PublishSubscribeChannelStatisticsLogger>(typeof(PublishSubscribeChannelStatisticsLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IMiddleware<MessageContext>, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IMiddleware<MessageContext>, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());
        }
    }
}
