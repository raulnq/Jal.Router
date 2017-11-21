using System;
using System.Net;
using System.Threading;
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
using Jal.Router.AzureStorage.Installer;
using Jal.Router.Impl;
using Jal.Router.Impl.Management;
using Jal.Router.Installer;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Logger.Extensions;
using Jal.Router.Logger.Installer;
using Jal.Router.Model;
using Jal.Router.Model.Management;
using Jal.Router.Tests.Impl;
using Jal.Router.Tests.Model;
using Jal.Settings.Installer;
using Microsoft.ServiceBus.Messaging;
using NUnit.Framework;

namespace Jal.Router.Tests.Integration
{
    public class Logger : ILogger<PointToPointChannelInfo>
    {
        public void Log(PointToPointChannelInfo info, DateTime date)
        {
            Console.WriteLine("Hi 23");
        }
    }
    [TestFixture]
    public class Tests
    {
        private IRouter _brokered;

     

        private IBus _bus;

        private IStartup _startup;

        [SetUp]
        public void SetUp()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            AssemblyFinder.Current = AssemblyFinder.Builder.UsePath(TestContext.CurrentContext.TestDirectory).Create;
            IWindsorContainer container = new WindsorContainer();
            var assemblies = AssemblyFinder.Current.GetAssembliesTagged<AssemblyTagAttribute>();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Install(new RouterInstaller(assemblies));
            container.Install(new ServiceLocatorInstaller());
            container.Install(new SettingsInstaller());
            container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(MessageHandler)).Named(typeof(MessageHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IMessageHandler<Message1>)).ImplementedBy(typeof(Message1Handler)).Named(typeof(Message1Handler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(OtherMessageHandler)).Named(typeof(OtherMessageHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(ILogger<PointToPointChannelInfo>)).ImplementedBy(typeof(Logger)).Named(typeof(Logger).FullName).LifestyleSingleton());
            container.Install(new AzureServiceBusRouterInstaller());
            //container.Install(new AzureStorageInstaller("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA=="));
            container.Install(new RouterLoggerInstaller());
            container.Register(Component.For(typeof (ILog)).Instance(LogManager.GetLogger("Cignium.Enigma.App")));
            _bus = container.Resolve<IBus>();
            _brokered = container.Resolve<IRouter>();
            _startup = container.Resolve<IStartup>();
            var config = container.Resolve<IConfiguration>();
            var monitor = container.Resolve<IMonitor>();
            config.AddMonitoringTask<PointToPointChannelMonitor>(5000);
            config.AddLogger<Logger, PointToPointChannelInfo>();
            monitor.Start();
            
            //config.UsingAzureServiceBus();
            //config.UsingCommonLogging();
            //_sagabrokered = container.Resolve<ISagaRouter<BrokeredMessage>>();
        }

        [Test]
        public void Validate_WithoutRuleName_Valid()
        {

            //_starter.Start();
            Thread.Sleep(
                30000);
            var bm = new BrokeredMessage(@"{""Name"":""Raul""}");
            bm.Properties.Add("origin","AB");
            var message = new Message();
            //_bus.Send(message, new Options() {EndPointName = "retry"});
            //_bus.Send(message, new Options() { EndPointName = "error" });
            //_brokered.Route<Message, BrokeredMessage>(bm);

            //_brokered.Route<Message>(bm,"error");

            _brokered.Route<Message, BrokeredMessage>(bm);

            //_brokered.RouteToSaga<Message, BrokeredMessage>(bm, "saga");

            var bm1 = new BrokeredMessage(@"{""Name1"":""Raul""}");
            bm1.Properties.Add("origin", "AB");

            //_brokered.RouteToSaga<Message1, BrokeredMessage>(bm1, "saga");
            //_router.Route(message, new Response() {Status = "Hi"});

            _bus.Send(message, new Options() {Id = "Id"});

            //_bus.Retry(message, new InboundMessageContext(), new LinearRetryPolicy(10,5));

            _bus.Publish(message, new Options() { Id = "Id" });
        }

        //[Test]
        //[TestCase("", 15)]
        //[TestCase(null, 1)]
        //[TestCase(" ", -1)]
        //[TestCase("  ", 0)]
        //public void Validate_WithoutRuleName_IsNotValid(string name, int age)
        //{
        //    var customer = new Customer
        //    {
        //        Name = name,
        //        Age = age
        //    };
        //    var validationResult = _requestRouter.Validate(customer);
        //    Assert.AreEqual(false, validationResult.IsValid);
        //    Assert.AreEqual(2, validationResult.Errors.Count);
        //}

        //[Test]
        //[TestCase("Name", 19)]
        //[TestCase("A", 10000)]
        //[TestCase("_", 999)]
        //public void Validate_WithRuleName_IsValid(string name, int age)
        //{
        //    var customer = new Customer
        //    {
        //        Name = name,
        //        Age = age
        //    };
        //    var validationResult = _requestRouter.Validate(customer, "Group");
        //    Assert.AreEqual(true, validationResult.IsValid);
        //}
    }
}

