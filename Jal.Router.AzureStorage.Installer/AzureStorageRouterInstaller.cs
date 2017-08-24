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

        private readonly string _partitionkeyheadername;

        private readonly string _rowkeyheadername;
        public AzureStorageRouterInstaller(string connectionstring, string sagastoragename = "sagas", string messagestorgename = "messages", string partitionkeyheadername = "partitionkey", string rowkeyheadername = "rowkey")
        {
            _connectionstring = connectionstring;

            _sagastoragename = sagastoragename;

            _messagestorgename = messagestorgename;

            _partitionkeyheadername = partitionkeyheadername;

            _rowkeyheadername = rowkeyheadername;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IStorage)).ImplementedBy(typeof(AzureTableStorage)).DependsOn(new { connectionstring =_connectionstring, sagastoragename = _sagastoragename, messagestorgename = _messagestorgename, partitionkeyheadername= _partitionkeyheadername, rowkeyheadername = _rowkeyheadername }).LifestyleSingleton());
        }
    }
}
