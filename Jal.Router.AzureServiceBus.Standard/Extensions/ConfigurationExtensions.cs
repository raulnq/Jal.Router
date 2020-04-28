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
                    .UsePointToPointResource<AzureManagementPointToPointResource>()
                    .UsePublishSubscribeResource<AzureManagementPublishSubscribeResource>()
                    .UseSubscriptionToPublishSubscribeResource<AzureManagementSubscriptionToPublishSubscribeResource>()
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
                    .UsePointToPointResource<AzureServiceBusPointToPointResource>()
                    .UsePublishSubscribeResource<AzureServiceBusPublishSubscribeResource>()
                    .UseSubscriptionToPublishSubscribeResource<AzureServiceBusSubscriptionToPublishSubscribeResource>()
                    .UseMessageAdapter<AzureServiceBusMessageAdapter>()
                    .AddParameter(p);
            }



        }


    }
}