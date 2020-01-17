using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemPointToPointChannelResource : AbstractFileSystemChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>
    {
        public FileSystemPointToPointChannelResource(IParameterProvider provider, IFileSystemTransport transport) : base(provider, transport)
        {
        }

        public override Task<bool> CreateIfNotExist(PointToPointChannelResource channel)
        {
            var path = _transport.CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> DeleteIfExist(PointToPointChannelResource channel)
        {
            var path = _transport.CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }
    }
}