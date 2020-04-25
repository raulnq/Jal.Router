﻿using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class InMemoryPointToPointResourceManager : AbstractInMemoryResourceManager
    {
        public InMemoryPointToPointResourceManager(IInMemoryTransport transport) : base(transport)
        {
        }

        public override Task<bool> CreateIfNotExist(Resource resource)
        {
            var name = _transport.CreateName(resource.ConnectionString, resource.Path);

            if (!_transport.Exists(name))
            {
                _transport.Create(name);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}