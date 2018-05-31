namespace Jal.Router.AzureServiceBus.Standard.Model
{
    public class ServiceBusConfiguration
    {

        public string ConnectionString { get; set; }

        public string ResourceGroupName { get; set; }

        public string ResourceName { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string SubscriptionId { get; set; }

        public string TenantId { get; set; }
    }
}
