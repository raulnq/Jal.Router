using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class InMemoryPointToPointChannelResourceManager : AbstractInMemoryChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>
    {
        public InMemoryPointToPointChannelResourceManager(IInMemoryTransport transport) : base(transport)
        {
        }

        public override Task<bool> CreateIfNotExist(PointToPointChannelResource channel)
        {
            var name = _transport.CreateName(channel.ConnectionString, channel.Path);

            if (!_transport.Exists(name))
            {
                _transport.Create(name);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}