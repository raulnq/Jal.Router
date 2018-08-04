using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureServiceBus.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UsingAzureServiceBus(this IConfiguration configuration)
        {
            configuration.UsingPointToPointChannel<AzureServiceBusQueue>();

            configuration.UsingPublishSubscribeChannel<AzureServiceBusTopic>();

            configuration.UsingRequestReplyChannel<AzureServiceBusSession>();

            configuration.UsingChannelManager<AzureServiceBusManager>();

            configuration.UsingMessageAdapter<BrokeredMessageAdapter>();

            configuration.UsingMessageSerializer<JsonMessageSerializer>();

            configuration.ChannelProviderName = "Azure Service Bus";
        }


    }
}