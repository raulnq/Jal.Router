using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Jal.Factory.Installer;
using Jal.Finder.Atrribute;
using Jal.Finder.Impl;
using Jal.Locator.CastleWindsor.Installer;
using Jal.Router.Installer;
using Jal.Router.Interface;
using Jal.Router.Tests.Model;
using Jal.Settings.Installer;
using NUnit.Framework;

namespace Jal.Router.Tests.Integration
{
    [TestFixture]
    public class Tests
    {
        private IMessageRouter _messageRouter;

        [SetUp]
        public void SetUp()
        {
            AssemblyFinder.Current = AssemblyFinder.Builder.UsePath(TestContext.CurrentContext.TestDirectory).Create;
            IWindsorContainer container = new WindsorContainer();
            var assemblies = AssemblyFinder.Current.GetAssembliesTagged<AssemblyTagAttribute>();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Install(new RouterInstaller(assemblies, assemblies));
            container.Install(new ServiceLocatorInstaller());
            container.Install(new SettingsInstaller());
            container.Install(new FactoryInstaller(assemblies));
            _messageRouter = container.Resolve<IMessageRouter>();
        }

        [Test]
        public void Validate_WithoutRuleName_Valid()
        {
            var request = new Request();
            _messageRouter.Route<Request>(request, "Route");
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

