using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Jal.Finder.Atrribute;
using Jal.Finder.Impl;
using Jal.Locator.CastleWindsor.Installer;
using Jal.Router.AzureServiceBus.Installer;
using Jal.Router.Impl;
using Jal.Router.Installer;
using Jal.Router.Interface;
using Jal.Router.Logger.Installer;
using Jal.Router.Model;
using Jal.Router.Tests.Impl;
using Jal.Router.Tests.Model;
using Jal.Settings.Installer;
using Microsoft.ServiceBus.Messaging;
using NUnit.Framework;

namespace Jal.Router.Tests.Integration
{
    [TestFixture]
    public class Tests
    {
        private IRouter _router;

        private IRouter<BrokeredMessage> _brokered;

        private IBus _bus;

        [SetUp]
        public void SetUp()
        {
            AssemblyFinder.Current = AssemblyFinder.Builder.UsePath(TestContext.CurrentContext.TestDirectory).Create;
            IWindsorContainer container = new WindsorContainer();
            var assemblies = AssemblyFinder.Current.GetAssembliesTagged<AssemblyTagAttribute>();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Install(new RouterInstaller(assemblies));
            container.Install(new ServiceLocatorInstaller());
            container.Install(new SettingsInstaller());
            container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(MessageHandler)).Named(typeof(MessageHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(OtherMessageHandler)).Named(typeof(OtherMessageHandler).FullName).LifestyleSingleton());
            container.Install(new AzureServiceBusRouterInstaller());
            container.Install(new RouterLoggerInstaller());
            _router = container.Resolve<IRouter>();
            container.Register(Component.For(typeof (ILog)).Instance(LogManager.GetLogger("Cignium.Enigma.App")));
            _bus = container.Resolve<IBus>();
            _brokered = container.Resolve<IRouter<BrokeredMessage>>();
        }

        [Test]
        public void Validate_WithoutRuleName_Valid()
        {
            var bm = new BrokeredMessage(@"{""Name"":""Raul""}");
            bm.Properties.Add("origin","AB");
            var message = new Message();

            //_router.Route<Message>(message);

            _brokered.Route<Message>(bm);

            //_router.Route(message, new Response() {Status = "Hi"});
            
            //_bus.Send(message, new Options() {Id = "Id"});

            //_bus.Retry(message, new InboundMessageContext(), new LinearRetryPolicy(10,5));

            //_bus.Publish(message, new Options() { MessageId = "Id" });
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

