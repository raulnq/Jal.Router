using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Jal.Finder.Atrribute;
using Jal.Finder.Impl;
using Jal.Locator.CastleWindsor.Installer;
using Jal.Router.AzureServiceBus.Installer;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Installer;
using Jal.Router.Interface;
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

        private IBrokeredMessageRouter _brokeredMessageRouter;

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
            container.Register(Component.For(typeof(IMessageHandler<Request>)).ImplementedBy(typeof(MessageHandler)).Named(typeof(MessageHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IMessageHandler<Request>)).ImplementedBy(typeof(OtherMessageHandler)).Named(typeof(OtherMessageHandler).FullName).LifestyleSingleton());
            container.Install(new BrokeredMessageRouterInstaller());
            _router = container.Resolve<IRouter>();
            container.Register(Component.For(typeof (ILog)).Instance(LogManager.GetLogger("Cignium.Enigma.App")));
            _brokeredMessageRouter = container.Resolve<IBrokeredMessageRouter>();
        }

        [Test]
        public void Validate_WithoutRuleName_Valid()
        {
            var bm = new BrokeredMessage(@"{""Name"":""Raul""}");

            var request = new Request();
            //_router.Route<Request>(request);
            _brokeredMessageRouter.Route<Request>(bm);
            //_router.Route(request, new Response() {Status = "Hi"});
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

