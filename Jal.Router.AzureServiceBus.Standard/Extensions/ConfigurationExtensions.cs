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
                    .UsePointToPointResourceManager<AzureManagementPointToPointResourceManager>()
                    .UsePublishSubscribeResourceManager<AzureManagementPublishSubscribeResourceManager>()
                    .UseSubscriptionToPublishSubscribeResourceManager<AzureManagementSubscriptionToPublishSubscribeResourceManager>()
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
                    .UsePointToPointResourceManager<AzureServiceBusPointToPointResourceManager>()
                    .UsePublishSubscribeResourceManager<AzureServiceBusPublishSubscribeResourceManager>()
                    .UseSubscriptionToPublishSubscribeResourceManager<AzureServiceBusSubscriptionToPublishSubscribeResourceManager>()
                    .UseMessageAdapter<AzureServiceBusMessageAdapter>()
                    .AddParameter(p);
            }



        }


    }
}