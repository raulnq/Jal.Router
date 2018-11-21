using Common.Logging;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using System;

namespace Jal.Router.Azure.Standard.LightInject.Installer.All
{
    public interface IHostBuilder
    {
        IHostBuilder UsingAzureServiceBus(IRouterConfigurationSource[] sources, string shutdownfile="", double autorenewtimeout=60, int maxconcurrentcalls=4);
        IHostBuilder UsingCommonLogging(ILog log);
        IHostBuilder UsingApplicationInsights(string applicationinsightskey="");
        IHostBuilder UsingAzureStorage(string connectionstring, string sagastoragename="sagas", string messagestoragename="messages", string tablenamesufix="", string container="");
        IHostBuilder UsingHeartBeatMonitor(int frequency);
        IHostBuilder UsingRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        IHostBuilder UsingBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor;
        IHostBuilder Using(Action<IConfiguration> setup);
        IHost Build();
    }
}