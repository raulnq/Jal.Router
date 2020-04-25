using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseAzureServiceBusAsTransport(this IConfiguration configuration, AzureServiceBusParameter parameter=null, bool useazureservicemanagement = true)
        {
            var p = new AzureServiceBusParameter();

            if (parameter != null)
            {
                p = parameter;
            }

            if (useazureservicemanagement)
            {
                return configuration
                    .SetTransportName("Azure Service Bus")
                    .UsePointToPointChannel<Impl.AzureServiceBusQueue>()
                    .UsePublishSubscribeChannel<Impl.AzureServiceBusTopic>()
                    .UseRequestReplyChannelFromPointToPointChannel<AzureServiceBusRequestReplyFromPointToPointChannel>()
                    .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                    .UsePointToPointResourceManager<AzureManagementPointToPointChannelResourceManager>()
                    .UsePublishSubscribeResourceManager<AzureManagementPublishSubscribeChannelResourceManager>()
                    .UseSubscriptionToPublishSubscribeResourceManager<AzureManagementSubscriptionToPublishSubscribeChannelResourceManager>()
                    .UseMessageAdapter<AzureServiceBusMessageAdapter>()
                    .AddParameter(p);
            }
            else
            {
                return configuration
                    .SetTransportName("Azure Service Bus")
                    .UsePointToPointChannel<Impl.AzureServiceBusQueue>()
                    .UsePublishSubscribeChannel<Impl.AzureServiceBusTopic>()
                    .UseRequestReplyChannelFromPointToPointChannel<AzureServiceBusRequestReplyFromPointToPointChannel>()
                    .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                    .UsePointToPointResourceManager<AzureServiceBusPointToPointChannelResourceManager>()
                    .UsePublishSubscribeResourceManager<AzureServiceBusPublishSubscribeChannelResourceManager>()
                    .UseSubscriptionToPublishSubscribeResourceManager<AzureServiceBusSubscriptionToPublishSubscribeChannelResourceManager>()
                    .UseMessageAdapter<AzureServiceBusMessageAdapter>()
                    .AddParameter(p);
            }



        }


    }
}