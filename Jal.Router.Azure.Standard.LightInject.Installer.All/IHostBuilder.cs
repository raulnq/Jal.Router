using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage;
using Jal.Router.Interface;
using System;

namespace Jal.Router.Azure.Standard.LightInject.Installer.All
{
    public interface IHostBuilder
    {
        IHostBuilder UseAzureServiceBus(Action<IRouterBuilder> builderaction, Action<AzureServiceBusParameter> action=null);
        IHostBuilder UseSerilog();
        IHostBuilder UseApplicationInsights(string applicationinsightskey="");
        IHostBuilder UseAzureStorage(Action<AzureStorageParameter> action = null);
        IHostBuilder UseHeartBeatMonitor(int frequency);
        IHostBuilder Use(Action<IConfiguration> action);
        IHost Build();
    }
}