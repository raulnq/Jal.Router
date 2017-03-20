using Jal.Router.AzureServiceBus.Model;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageRouterConfigurationSource
    {
        EndPoint[] Source();
    }
}