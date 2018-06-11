using Common.Logging;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureServiceBus.Standard.LightInject.Installer.All
{
    public interface IHostBuilder
    {
        IHostBuilder UsingAzureServiceBus(IRouterConfigurationSource[] sources, string shutdownfile="", double autorenewtimeout=60, int maxconcurrentcalls=4);
        IHostBuilder UsingCommonLogging(ILog log);
        IHostBuilder UsingApplicationInsights(string applicationinsightskey);
        IHostBuilder UsingAzureStorage(string connectionstring, string sagastoragename="sagas", string messagestoragename="messages", string tablenamesufix="");
        IHostBuilder UsingHeartBeatMonitor(int frequency);
        IHostBuilder UsingRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        IHost Build();
    }
}