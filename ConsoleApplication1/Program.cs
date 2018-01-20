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
using Jal.Router.AzureServiceBus.Extensions;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Installer;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.AzureStorage.Installer;
using Jal.Router.Impl;
using Jal.Router.Impl.Management;
using Jal.Router.Installer;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Logger.Installer;
using Jal.Router.Model;
using Jal.Router.Tests.Impl;
using Jal.Router.Tests.Model;
using Jal.Settings.Installer;
using Microsoft.ServiceBus.Messaging;
using Component = Castle.MicroKernel.Registration.Component;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {



            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            AssemblyFinder.Current = AssemblyFinder.Builder.UsePath(AppDomain.CurrentDomain.BaseDirectory).Create;
            IWindsorContainer container = new WindsorContainer();
            var assemblies = AssemblyFinder.Current.GetAssembliesTagged<AssemblyTagAttribute>();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Install(new RouterInstaller(assemblies, "/"));
            container.Install(new ServiceLocatorInstaller());
            container.Install(new SettingsInstaller());
            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerHandler)).Named(typeof(TriggerHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestHandler)).Named(typeof(RequestHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowAHandler)).Named(typeof(TriggerFlowAHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestToSendAppAHandler)).Named(typeof(RequestToSendAppAHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(ResponseToSendAppBHandler)).Named(typeof(ResponseToSendAppBHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowBHandler)).Named(typeof(TriggerFlowBHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestToSendAppCHandler)).Named(typeof(RequestToSendAppCHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(ResponseToSendAppDHandler)).Named(typeof(ResponseToSendAppDHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowCHandler)).Named(typeof(TriggerFlowCHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend,Data>)).ImplementedBy(typeof(RequestToSendAppEHandler)).Named(typeof(RequestToSendAppEHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppFHandler)).Named(typeof(ResponseToSendAppFHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(RequestToSendAppXHandler)).Named(typeof(RequestToSendAppXHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppHHandler)).Named(typeof(ResponseToSendAppHHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(RequestToSendAppZHandler)).Named(typeof(RequestToSendAppZHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowDHandler)).Named(typeof(TriggerFlowDHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppGHandler)).Named(typeof(ResponseToSendAppGHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowEHandler)).Named(typeof(TriggerFlowEHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppJHandler)).Named(typeof(ResponseToSendAppJHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppIHandler)).Named(typeof(ResponseToSendAppIHandler).FullName).LifestyleSingleton());
            //container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(OtherMessageHandler)).Named(typeof(OtherMessageHandler).FullName).LifestyleSingleton());


            //container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(SagaInput1HandlerMessageHandler)).Named(typeof(SagaInput1HandlerMessageHandler).FullName).LifestyleSingleton());
            //container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(SagaInputTopicHandlerMessageHandler)).Named(typeof(SagaInputTopicHandlerMessageHandler).FullName).LifestyleSingleton());
            //container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(SagaInputTopic2HandlerMessageHandler)).Named(typeof(SagaInputTopic2HandlerMessageHandler).FullName).LifestyleSingleton());

            container.Install(new AzureServiceBusRouterInstaller(8));
            container.Install(new AzureStorageRouterInstaller("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==", "sagatests", "messagestests", DateTime.UtcNow.ToString("yyyyMMdd")));
            container.Install(new RouterLoggerInstaller());
            container.Register(Component.For(typeof(ILog)).Instance(LogManager.GetLogger("Cignium.Enigma.App")));
            var sagabrokered = container.Resolve<IRouter>();

            //config.UsingCommonLogging();

            var host = container.Resolve<IHost>();

            host.Configuration.UsingAzureServiceBus();

            host.Configuration.UsingAzureStorage();

            host.Configuration.ApplicationName = "App";

            host.Configuration.AddMonitoringTask<HeartBeatMonitor>(60000);

            host.Configuration.UsingShutdownWatcher<ShutdownFileWatcher>();

            host.RunAndBlock();

            Console.ReadLine();

            //var bm = new BrokeredMessage(@"{""Name"":""Raul""}");
            //bm.Properties.Add("origin", "AB");


            //sagabrokered.Route<Message, BrokeredMessage>(bm);

            //var bm1 = new BrokeredMessage(@"{""Name1"":""Raul Naupari""}");
            //bm1.Properties.Add("origin", "ABC");
            ////{"partitionkey":"20170822_saga_f0dd186a-3043-4f32-b437-3d5d283b8f88","rowkey":"e5a5c8be-ce35-45e4-9a8c-3d8b539513dc"}
            ////{ "partitionkey":"20170821_944e53c5-b3eb-43f1-bc87-e5bd4260c21a","rowkey":"f274f66c-095e-48ce-8f3c-6fc7a2b0ef39"}
            ////{"partitionkey":"20170821_737d3a2e-b22c-4ec0-92d9-82474df6757f","rowkey":"bd09b4ed-6485-4293-b568-8e021ec8179c"}
            //bm1.Properties.Add("sagaid", "20171016_saga@b7250358-a128-4f6a-b4e8-027506cbbe7f@20171016");
            //sagabrokered.RouteToSaga<Message1, BrokeredMessage>(bm1, "saga");

            //var bm2 = new BrokeredMessage(@"{""Name1"":""Raul Naupari""}");
            //bm2.Properties.Add("origin", "xcd");
            ////{"partitionkey":"20170822_saga_f0dd186a-3043-4f32-b437-3d5d283b8f88","rowkey":"e5a5c8be-ce35-45e4-9a8c-3d8b539513dc"}
            ////{ "partitionkey":"20170821_944e53c5-b3eb-43f1-bc87-e5bd4260c21a","rowkey":"f274f66c-095e-48ce-8f3c-6fc7a2b0ef39"}
            ////{"partitionkey":"20170821_737d3a2e-b22c-4ec0-92d9-82474df6757f","rowkey":"bd09b4ed-6485-4293-b568-8e021ec8179c"}
            //bm2.Properties.Add("sagaid", "20170927_saga@69864a1a-defa-45c8-aa3c-c6b03b026b93@20170927");
            //sagabrokered.Route<Message1, BrokeredMessage>(bm2, "saga");
        }
    }
}
