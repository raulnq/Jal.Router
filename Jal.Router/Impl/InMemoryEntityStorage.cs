using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public override Task<string> Create(SagaData sagadata)
        {
            var id = Guid.NewGuid().ToString();

            _sagas.Add(id, sagadata);

            return Task.FromResult(id);
        }

        public override Task Update(SagaData sagadata)
        {
            _sagas[sagadata.Id] = sagadata;

            return Task.CompletedTask;
        }

        public override Task<SagaData> Get(string id)
        {
            return Task.FromResult(_sagas[id]);
        }
    }
}