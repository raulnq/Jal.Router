using Jal.Router.Interface;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class StorageFacade : IStorageFacade
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;
        public StorageFacade(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public void Save<TData>(MessageContext context, TData data) where TData : class, new()
        {
            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            storage.UpdateSaga(context, data);
        }
    }
}