using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;


namespace Jal.Router.Impl
{
    public class InMemoryEntityStorage : AbstractEntityStorage
    {
        private readonly Dictionary<string, SagaData> _sagas;

        public InMemoryEntityStorage()
        {
            _sagas = new Dictionary<string, SagaData>();
        }

        public override Task<string> Insert(SagaData sagadata, IMessageSerializer serializer)
        {
            var id = Guid.NewGuid().ToString();

            _sagas.Add(id, sagadata);

            return Task.FromResult(id);
        }

        public override Task Update(SagaData sagadata, IMessageSerializer serializer)
        {
            _sagas[sagadata.Id] = sagadata;

            return Task.CompletedTask;
        }

        public override Task<SagaData> Get(string id, IMessageSerializer serializer)
        {
            return Task.FromResult(_sagas[id]);
        }
    }
}