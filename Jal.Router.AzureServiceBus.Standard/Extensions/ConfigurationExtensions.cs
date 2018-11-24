using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UsingAzureServiceBus(this IConfiguration configuration)
        {
            configuration.UsePointToPointChannel<AzureServiceBusQueue>();

            configuration.UsePublishSubscribeChannel<AzureServiceBusTopic>();

            configuration.UseRequestReplyChannel<AzureServiceBusSession>();

            configuration.UseChannelManager<AzureServiceBusManager>();

            configuration.UseMessageAdapter<MessageAdapter>();

            configuration.UseMessageSerializer<JsonMessageSerializer>();

            configuration.ChannelProviderName = "Azure Service Bus";
        }


    }
}