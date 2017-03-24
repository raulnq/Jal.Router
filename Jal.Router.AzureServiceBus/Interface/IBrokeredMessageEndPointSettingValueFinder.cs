namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageEndPointSettingValueFinder
    {
        string Find(string name);
    }
}