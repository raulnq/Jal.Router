using System;
using Common.Logging;
using Jal.Locator.LightInject.Installer;
using Jal.Router.ApplicationInsights.Extensions;
using Jal.Router.ApplicationInsights.LightInject.Installer;
using Jal.Router.AzureServiceBus.Standard.Extensions;
using Jal.Router.AzureStorage.LightInject.Installer;
using Jal.Router.Interface.Management;
using Jal.Router.LightInject.Installer;
using Jal.Router.Logger.LightInject.Installer;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.Impl.Management;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Logger.Extensions;
using LightInject;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Jal.Router.AzureServiceBus.Standard.LightInject.Installer.All
{
    public class HostBuilder : IHostBuilder
    {
        private readonly HostBuilderParameter _parameter;

        public HostBuilder(ServiceContainer container, string applicationname)
        {
            _parameter = new HostBuilderParameter()
            {
                Container = container,
                ApplicationName = applicationname
            };
        }

        public static IHostBuilder Create(ServiceContainer container, string applicationname)
        {
            return new HostBuilder(container, applicationname);
        }

        public IHostBuilder UsingAzureServiceBus(IRouterConfigurationSource[] sources, string shutdownfile = "",
            double autorenewtimeout = 60, int maxconcurrentcalls = 4)
        {
            _parameter.Sources = sources;
            _parameter.ShutdownFile = shutdownfile;
            _parameter.AutoRenewTimeout = autorenewtimeout;
            _parameter.MaxConcurrentCalls = maxconcurrentcalls;
            return this;
        }

        public IHostBuilder UsingCommonLogging(ILog log)
        {
            _parameter.Log = log;
            return this;
        }

        public IHostBuilder UsingApplicationInsights(string applicationinsightskey)
        {
            _parameter.ApplicationInsightsKey = applicationinsightskey;
            return this;
        }

        public IHostBuilder UsingAzureStorage(string connectionstring, string sagastoragename = "sagas",
            string messagestoragename = "messages", string tablenamesufix = "")
        {
            _parameter.StorageConnectionString = connectionstring;
            _parameter.SagaStorageName = sagastoragename;
            _parameter.MessageStorageName = messagestoragename;
            _parameter.TableNameSufix = tablenamesufix;
            return this;
        }

        public IHostBuilder UsingHeartBeatMonitor(int frequency)
        {
            _parameter.HeartBeatFrequency = frequency;
            return this;
        }

        public IHostBuilder UsingRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor
        {
            _parameter.RouterInterceptorType = typeof(TRouterInterceptor);
            return this;
        }

        public IHost Build()
        {
            _parameter.Container.RegisterFrom<ServiceLocatorCompositionRoot>();

            _parameter.Container.RegisterRouter(_parameter.Sources, _parameter.ShutdownFile);

            _parameter.Container.RegisterAzureServiceBusRouter(_parameter.MaxConcurrentCalls, TimeSpan.FromMinutes(_parameter.AutoRenewTimeout));

            if (_parameter.Log != null)
            {
                _parameter.Container.Register(x => _parameter.Log, new PerContainerLifetime());

                _parameter.Container.RegisterRouterLogger();
            }

            if (!string.IsNullOrWhiteSpace(_parameter.ApplicationInsightsKey))
            {
                _parameter.Container.Register<TelemetryClient>(new PerContainerLifetime());

                _parameter.Container.RegisterApplicationInsights();

                TelemetryConfiguration.Active.InstrumentationKey = _parameter.ApplicationInsightsKey;
            }

            if (!string.IsNullOrWhiteSpace(_parameter.StorageConnectionString))
            {
                _parameter.Container.RegisterAzureStorage(_parameter.StorageConnectionString, _parameter.SagaStorageName, _parameter.MessageStorageName, _parameter.TableNameSufix);
            }

            if (_parameter.RouterInterceptorType != null)
            {
                _parameter.Container.Register(typeof(IRouterInterceptor), _parameter.RouterInterceptorType, _parameter.RouterInterceptorType.FullName, new PerContainerLifetime());
            }
            
            var host = _parameter.Container.GetInstance<IHost>();

            host.Configuration.UsingAzureServiceBus();

            if (_parameter.Log != null)
            {
                host.Configuration.UsingCommonLogging();
            }

            host.Configuration.ApplicationName = _parameter.ApplicationName;

            if (!string.IsNullOrWhiteSpace(_parameter.StorageConnectionString))
            {
                host.Configuration.UsingAzureStorage();
            }

            host.Configuration.Storage.IgnoreExceptionOnSaveMessage = true;

            if (!string.IsNullOrWhiteSpace(_parameter.ApplicationInsightsKey))
            {
                host.Configuration.UsingApplicationInsights();
            }

            if (_parameter.RouterInterceptorType != null)
            {
                host.Configuration.RouterInterceptorType = _parameter.RouterInterceptorType;
            }

            host.Configuration.AddMonitoringTask<HeartBeatMonitor>(_parameter.HeartBeatFrequency);

            host.Configuration.UsingShutdownWatcher<ShutdownFileWatcher>();

            return host;
        }
    }
}
