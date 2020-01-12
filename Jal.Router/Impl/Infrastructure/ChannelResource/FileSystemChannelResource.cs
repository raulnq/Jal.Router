using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemChannelResource : AbstractChannelResource, IChannelResource
    {
        private readonly FileSystemParameter _parameter;

        private readonly IFileSystemTransport _transport;

        public FileSystemChannelResource(IParameterProvider provider, IFileSystemTransport transport)
        {
            _parameter = provider.Get<FileSystemParameter>();

            _transport = transport;
        }

        public override Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel)
        {
            var path = _transport.CreatePublishSubscribeChannelPath(_parameter,  channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> CreateIfNotExist(PointToPointChannelResource channel)
        {
            var path = _transport.CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public override Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }

        public override Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel)
        {
            var path = _transport.CreatePublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }

        public override Task<bool> DeleteIfExist(PointToPointChannelResource channel)
        {
            var path = _transport.CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }
    }
}