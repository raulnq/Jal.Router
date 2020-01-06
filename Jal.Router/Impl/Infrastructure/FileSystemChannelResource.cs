using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemChannelResource : IChannelResource
    {
        private readonly FileSystemParameter _parameter;

        private readonly IFileSystem _filesystem;

        public FileSystemChannelResource(IParameterProvider provider, IFileSystem filesystem)
        {
            _parameter = provider.Get<FileSystemParameter>();
            _filesystem = filesystem;
        }

        public Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = _filesystem.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            return Task.FromResult(_filesystem.CreateDirectory(path));
        }

        public Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel)
        {
            var path = _filesystem.CreatePublishSubscribeChannelPath(_parameter,  channel.ConnectionString, channel.Path);

            return Task.FromResult(_filesystem.CreateDirectory(path));
        }

        public Task<bool> CreateIfNotExist(PointToPointChannelResource channel)
        {
            var path = _filesystem.CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_filesystem.CreateDirectory(path));
        }

        public Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = _filesystem.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            return Task.FromResult(_filesystem.DeleteDirectory(path));
        }

        public Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel)
        {
            var path = _filesystem.CreatePublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_filesystem.DeleteDirectory(path));
        }

        public Task<bool> DeleteIfExist(PointToPointChannelResource channel)
        {
            var path = _filesystem.CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_filesystem.DeleteDirectory(path));
        }

        public Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannelResource channel)
        {
            return Task.FromResult(default(PublishSubscribeChannelStatistics));
        }

        public Task<PointToPointChannelStatistics> Get(PointToPointChannelResource channel)
        {
            return Task.FromResult(default(PointToPointChannelStatistics));
        }

        public Task<SubscriptionToPublishSubscribeChannelStatistics> Get(SubscriptionToPublishSubscribeChannelResource channel)
        {
            return Task.FromResult(default(SubscriptionToPublishSubscribeChannelStatistics));
        }
    }
}