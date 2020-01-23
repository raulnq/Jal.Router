using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemPublishSubscribeChannelResourceManager : AbstractFileSystemChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>
    {
        public FileSystemPublishSubscribeChannelResourceManager(IParameterProvider provider, IFileSystemTransport transport) : base(provider, transport)
        {
        }

        public override Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel)
        {
            var path = _transport.CreatePublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel)
        {
            var path = _transport.CreatePublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }
    }
}