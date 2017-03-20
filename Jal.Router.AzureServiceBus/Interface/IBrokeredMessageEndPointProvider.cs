using Jal.Router.AzureServiceBus.Model;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageEndPointProvider
    {
        EndPoint[] Provide<TContent>(string name = "");
    }
}