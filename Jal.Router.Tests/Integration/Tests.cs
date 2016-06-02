using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Jal.Factory.Installer;
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
        private IRequestRouter _requestRouter;

        [SetUp]
        public void SetUp()
        {
            AssemblyFinder.Current = AssemblyFinder.Builder.UsePath(TestContext.CurrentContext.TestDirectory).Create;
            IWindsorContainer container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Install(new RouterInstaller( AssemblyFinder.Current.GetAssemblies("Sender"), AssemblyFinder.Current.GetAssemblies("SenderSource")));
            container.Install(new ServiceLocatorInstaller());
            container.Install(new SettingsInstaller());
            container.Install(new FactoryInstaller(AssemblyFinder.Current.GetAssemblies("FactorySource")));
            _requestRouter = container.Resolve<IRequestRouter>();
        }

        [Test]
        public void Validate_WithoutRuleName_Valid()
        {
            var request = new Request();
            var response = _requestRouter.Route<Request, Response>(request, "Route");
            Assert.AreEqual(null, response);
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

