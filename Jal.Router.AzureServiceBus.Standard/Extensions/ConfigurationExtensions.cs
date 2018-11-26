using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseAzureServiceBus(this IConfiguration configuration, AzureServiceBusParameter parameter=null)
        {
            var p = new AzureServiceBusParameter();

            if (parameter != null)
            {
                p = parameter;
            }

            return configuration
                .SetChannelProviderName("Azure Service Bus")
                .UsePointToPointChannel<AzureServiceBusQueue>()
                .UsePublishSubscribeChannel<AzureServiceBusTopic>()
                .UseRequestReplyChannel<AzureServiceBusSession>()
                .UseChannelManager<AzureServiceBusManager>()
                .UseMessageAdapter<MessageAdapter>()
                .UseMessageSerializer<JsonMessageSerializer>()
                .AddParameter(p);
        }


    }
}