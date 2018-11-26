using Common.Logging;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using System;

namespace Jal.Router.Azure.Standard.LightInject.Installer.All
{
    public interface IHostBuilder
    {
        IHostBuilder UseAzureServiceBus(IRouterConfigurationSource[] sources, Action<AzureServiceBusParameter> action=null);
        IHostBuilder UseCommonLogging(ILog log);
        IHostBuilder UseApplicationInsights(string applicationinsightskey="");
        IHostBuilder UseAzureStorage(Action<AzureStorageParameter> action = null);
        IHostBuilder UseHeartBeatMonitor(int frequency);
        IHostBuilder UseRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        IHostBuilder UseBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor;
        IHostBuilder Use(Action<IConfiguration> action);
        IHost Build();
    }
}