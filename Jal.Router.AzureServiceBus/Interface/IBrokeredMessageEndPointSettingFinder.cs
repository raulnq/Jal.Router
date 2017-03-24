using Jal.Router.AzureServiceBus.Model;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageEndPointSettingFinder<in T>
    {
        BrokeredMessageEndPoint Find(T record);
    }
}