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
        IHostBuilder UseAzureServiceBus(IRouterConfigurationSource[] sources, string shutdownfile="", double autorenewtimeout=60, int maxconcurrentcalls=4);
        IHostBuilder UseCommonLogging(ILog log);
        IHostBuilder UseApplicationInsights(string applicationinsightskey="");
        IHostBuilder UseAzureStorage(string connectionstring, string sagastoragename="sagas", string messagestoragename="messages", string tablenamesufix="", string container="");
        IHostBuilder UseHeartBeatMonitor(int frequency);
        IHostBuilder UseRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        IHostBuilder UseBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor;
        IHostBuilder Use(Action<IConfiguration> setup);
        IHost Build();
    }
}