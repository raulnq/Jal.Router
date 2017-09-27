using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureStorage.Impl;
using Jal.Router.Interface;

namespace Jal.Router.AzureStorage.Installer
{
    public class AzureStorageRouterInstaller : IWindsorInstaller
    {
        private readonly string _connectionstring;

        private readonly string _sagastoragename;

        private readonly string _messagestorgename;

        private readonly string _tablenamesufix;
        public AzureStorageRouterInstaller(string connectionstring, string sagastoragename = "sagas", string messagestorgename = "messages", string tablenamesufix = "")
        {
            _connectionstring = connectionstring;

            _sagastoragename = sagastoragename;

            _messagestorgename = messagestorgename;

            _tablenamesufix = tablenamesufix;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IStorage)).ImplementedBy(typeof(AzureTableStorage)).DependsOn(new { connectionstring =_connectionstring, sagastoragename = _sagastoragename, messagestorgename = _messagestorgename, tablenamesufix = _tablenamesufix }).LifestyleSingleton());
            container.Register(Component.For(typeof(IStep)).ImplementedBy(typeof(AzureStorageSetupStep)).Named(typeof(AzureStorageSetupStep).FullName).DependsOn(new { connectionstring = _connectionstring, sagastoragename = _sagastoragename, messagestorgename = _messagestorgename, tablenamesufix = _tablenamesufix }).LifestyleSingleton());
        }
    }
}
