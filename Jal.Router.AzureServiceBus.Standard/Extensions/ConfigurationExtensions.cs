using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration AddAzureServiceBusAsTransport(this IConfiguration configuration, AzureServiceBusParameter parameter=null)
        {
            var p = new AzureServiceBusParameter();

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