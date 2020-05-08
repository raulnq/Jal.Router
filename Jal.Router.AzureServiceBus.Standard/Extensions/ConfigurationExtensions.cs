using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration AddAzureServiceBusAsDefaultTransport(this IConfiguration configuration, AzureServiceBusChannelConnection connection=null)
        {
            var p = new AzureServiceBusChannelConnection();

            if (connection != null)
            {
                p = connection;
            }

            return configuration
                .SetDefaultTransportName("Azure Service Bus")
                .UsePointToPointChannel<AzureServiceBusQueue>()
                .UsePublishSubscribeChannel<AzureServiceBusTopic>()
                .UseSubscriptionToPublishSubscribeChannel<AzureServiceBusSubscription>()
                .UseMessageAdapter<AzureServiceBusMessageAdapter>()
                .AddParameter(p);
        }


    }
}