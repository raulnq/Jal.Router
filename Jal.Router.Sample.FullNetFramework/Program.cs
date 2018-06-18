using System;
using System.ComponentModel;
using System.Net;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Jal.Finder.Atrribute;
using Jal.Finder.Impl;
using Jal.Locator.CastleWindsor.Installer;
using Jal.Locator.LightInject.Installer;
using Jal.Router.AzureServiceBus.Extensions;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Installer;
using Jal.Router.AzureServiceBus.LightInject.Installer;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.AzureStorage.Installer;
using Jal.Router.Impl;
using Jal.Router.Extensions;
using Jal.Router.Impl.Management;
using Jal.Router.Installer;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.LightInject.Installer;
using Jal.Router.Logger.Installer;
using Jal.Router.Model;
using Jal.Router.Tests.Impl;
using Jal.Router.Tests.Model;
using Jal.Settings.Installer;
using LightInject;
using Microsoft.ServiceBus.Messaging;
using Component = Castle.MicroKernel.Registration.Component;


namespace Jal.Router.Sample.FullNetFramework
{
    class Program
    {
        public class Message
        {
            public string Name { get; set; }
        }
        public interface IMessageHandler
        {
            void Handle(Message message);
        }

        public class MessageHandler : IMessageHandler
        {
            public void Handle(Message message)
            {
                Console.WriteLine("Hello world!!");
            }
        }

        public class RouterConfigurationSourceSample : AbstractRouterConfigurationSource
        {
            public RouterConfigurationSourceSample()
            {
                RegisterHandler<IMessageHandler>("handler")
                    .ToListen(builder =>
                    {
                        builder.AddPointToPointChannel("testqueue", "Endpoint=sb://demo-8.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q2Gz996OsfNkE0IP39AxYDlNVEnHf3IxrNk4IEkGI/Y=");
                        builder.AddPointToPointChannel("testqueue2", "Endpoint=sb://demo-8.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q2Gz996OsfNkE0IP39AxYDlNVEnHf3IxrNk4IEkGI/Y=");
                        builder.AddPublishSubscribeChannel("testtopic", "subs", "Endpoint=sb://demo-8.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q2Gz996OsfNkE0IP39AxYDlNVEnHf3IxrNk4IEkGI/Y=");
                    })
                    .ForMessage<Message>().Using<MessageHandler>(x =>
                    {
                        x.With(((request, handler, context) => handler.Handle(request)));
                    });

                //RegisterHandler<IMessageHandler>("handler")
                //.ToListenQueue<IMessageHandler, AppSettingValueSettingFinder>("testqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                //.ForMessage<Message>().Using<MessageHandler>(x =>
                //{
                //    x.With(((request, handler, context) => handler.Handle(request)));
                //});
                
                //RegisterOrigin("appflowc", "789");

                //RegisterEndPoint("appe")
                // .ForMessage<RequestToSend>()
                // .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appequeue");


            }
        }
        static void Main(string[] args)
        {
            var container = new ServiceContainer();
            container.RegisterRouter(new IRouterConfigurationSource[] { new RouterConfigurationSourceSample() });
            container.RegisterFrom<ServiceLocatorCompositionRoot>();
            container.RegisterAzureServiceBusRouter();

            container.Register<IMessageHandler, MessageHandler>(typeof(MessageHandler).FullName, new PerContainerLifetime());

            var host = container.GetInstance<IHost>();
            host.Configuration.UsingAzureServiceBus();
            host.Run();
            Console.ReadLine();
        }

        //static void Main(string[] args)
        //{
        //    var container = new WindsorContainer();
        //    container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

        //    container.Install(new RouterInstaller(new IRouterConfigurationSource[] {new RouterConfigurationSourceSample()}));
        //    container.Install(new AzureServiceBusRouterInstaller());
        //    container.Register(Component.For(typeof(IMessageHandler)).ImplementedBy(typeof(MessageHandler)).Named(typeof(MessageHandler).FullName).LifestyleSingleton());


        //    container.Install(new ServiceLocatorInstaller());
        //    var host = container.Resolve<IHost>();
        //    host.Configuration.UsingAzureServiceBus();
        //    host.Run();
        //    Console.ReadLine();
        //}

        //static void Main(string[] args)
        //{



        //    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        //    AssemblyFinder.Current = AssemblyFinder.Builder.UsePath(AppDomain.CurrentDomain.BaseDirectory).Create;
        //    IWindsorContainer container = new WindsorContainer();
        //    var assemblies = AssemblyFinder.Current.GetAssembliesTagged<AssemblyTagAttribute>();
        //    container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
        //    container.Install(new RouterInstaller(assemblies, "/"));
        //    container.Install(new ServiceLocatorInstaller());
        //    container.Install(new SettingsInstaller());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerHandler)).Named(typeof(TriggerHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestHandler)).Named(typeof(RequestHandler).FullName).LifestyleSingleton());

        //    container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowAHandler)).Named(typeof(TriggerFlowAHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestToSendAppAHandler)).Named(typeof(RequestToSendAppAHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(ResponseToSendAppBHandler)).Named(typeof(ResponseToSendAppBHandler).FullName).LifestyleSingleton());

        //    container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowBHandler)).Named(typeof(TriggerFlowBHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestToSendAppCHandler)).Named(typeof(RequestToSendAppCHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(ResponseToSendAppDHandler)).Named(typeof(ResponseToSendAppDHandler).FullName).LifestyleSingleton());

        //    container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowCHandler)).Named(typeof(TriggerFlowCHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend,Data>)).ImplementedBy(typeof(RequestToSendAppEHandler)).Named(typeof(RequestToSendAppEHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppFHandler)).Named(typeof(ResponseToSendAppFHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(RequestToSendAppXHandler)).Named(typeof(RequestToSendAppXHandler).FullName).LifestyleSingleton());

        //    container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppHHandler)).Named(typeof(ResponseToSendAppHHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(RequestToSendAppZHandler)).Named(typeof(RequestToSendAppZHandler).FullName).LifestyleSingleton());

        //    container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowDHandler)).Named(typeof(TriggerFlowDHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppGHandler)).Named(typeof(ResponseToSendAppGHandler).FullName).LifestyleSingleton());

        //    container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowEHandler)).Named(typeof(TriggerFlowEHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppJHandler)).Named(typeof(ResponseToSendAppJHandler).FullName).LifestyleSingleton());
        //    container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppIHandler)).Named(typeof(ResponseToSendAppIHandler).FullName).LifestyleSingleton());

        //    container.Install(new AzureServiceBusRouterInstaller(8));
        //    container.Install(new AzureStorageRouterInstaller("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==", "sagatests", "messagestests", DateTime.UtcNow.ToString("yyyyMMdd")));
        //    container.Install(new RouterLoggerInstaller());
        //    container.Register(Component.For(typeof(ILog)).Instance(LogManager.GetLogger("Cignium.Enigma.App")));

        //    //config.UsingCommonLogging();

        //    var host = container.Resolve<IHost>();

        //    host.Configuration.UsingAzureServiceBus();

        //    host.Configuration.UsingAzureStorage();

        //    host.Configuration.ApplicationName = "App";

        //    host.Configuration.AddMonitoringTask<HeartBeatMonitor>(60000);

        //    host.Configuration.UsingShutdownWatcher<ShutdownFileWatcher>();

        //    host.RunAndBlock();

        //    Console.ReadLine();
        //}
    }
}
