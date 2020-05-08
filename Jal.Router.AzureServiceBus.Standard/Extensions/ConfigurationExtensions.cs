using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration AddAzureServiceBusAsTransport(this IConfiguration configuration, AzureServiceBusChannelConnection parameter=null)
        {
            var p = new AzureServiceBusChannelConnection();

            if (parameter != null)
            {
                p = parameter;
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