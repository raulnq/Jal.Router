using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class RouterBuilderExtensions
    {
        public static void AddAzureServiceBus(this IRouterBuilder builder)
        {
            builder.AddPointToPointChannel<AzureServiceBusQueue>();

            builder.AddPublishSubscribeChannel<AzureServiceBusTopic>();

            builder.AddSubscriptionToPublishSubscribeChannel<AzureServiceBusSubscription>();

            builder.AddMessageAdapter<AzureServiceBusMessageAdapter>();
        }
    }
}
