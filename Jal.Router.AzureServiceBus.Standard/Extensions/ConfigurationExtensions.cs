using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseAzureServiceBusAsTransport(this IConfiguration configuration, AzureServiceBusParameter parameter=null, bool useazureservicemanagemet = true)
        {
            var p = new AzureServiceBusParameter();

            if (parameter != null)
            {
                p = parameter;
            }

            if (useazureservicemanagemet)
            {
                return configuration
                    .SetChannelProviderName("Azure Service Bus")
                    .UsePointToPointChannel<Impl.AzureServiceBusQueue>()
                    .UsePublishSubscribeChannel<Impl.AzureServiceBusTopic>()
                    .UseRequestReplyChannelFromPointToPointChannel<AzureServiceBusRequestReplyFromPointToPointChannel>()
                    .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                    .UsePointToPointChannelResourceManager<AzureManagementPointToPointChannelResourceManager>()
                    .UsePublishSubscribeChannelResourceManager<AzureManagementPublishSubscribeChannelResourceManager>()
                    .UseSubscriptionToPublishSubscribeChannelResourceManager<AzureManagementSubscriptionToPublishSubscribeChannelResourceManager>()
                    .UseMessageAdapter<AzureServiceBusMessageAdapter>()
                    .AddParameter(p);
            }
            else
            {
                return configuration
                    .SetChannelProviderName("Azure Service Bus")
                    .UsePointToPointChannel<Impl.AzureServiceBusQueue>()
                    .UsePublishSubscribeChannel<Impl.AzureServiceBusTopic>()
                    .UseRequestReplyChannelFromPointToPointChannel<AzureServiceBusRequestReplyFromPointToPointChannel>()
                    .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                    .UsePointToPointChannelResourceManager<AzureServiceBusPointToPointChannelResourceManager>()
                    .UsePublishSubscribeChannelResourceManager<AzureServiceBusPublishSubscribeChannelResourceManager>()
                    .UseSubscriptionToPublishSubscribeChannelResourceManager<AzureServiceBusSubscriptionToPublishSubscribeChannelResourceManager>()
                    .UseMessageAdapter<AzureServiceBusMessageAdapter>()
                    .AddParameter(p);
            }



        }


    }
}