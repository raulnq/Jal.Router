using Jal.Router.AzureServiceBus.Model;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageEndPointSettingProvider
    {
        BrokeredMessageEndPoint Provide<T>(EndPoint endPoint, T record);
    }
}