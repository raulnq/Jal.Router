using Jal.Router.Interface;
using Jal.Router.Model;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemChannelResource : IChannelResource
    {
        private readonly FileSystemParameter _parameter;

        public FileSystemChannelResource(IParameterProvider provider)
        {
            _parameter = provider.Get<FileSystemParameter>();
        }

        public static byte[] GetHash(string inputString)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string CreatePointToPointChannelPath(FileSystemParameter parameter, string connectionstring, string path)
        {
            var hash = GetHashString(connectionstring);

            return Path.Combine(parameter.Path, "queues", hash, path);
        }

        public static string CreatePublishSubscribeChannelPath(FileSystemParameter parameter, string connectionstring, string path)
        {
            var hash = GetHashString(connectionstring);

            return Path.Combine(parameter.Path, "topics", hash, path);
        }

        public static string CreateSubscriptionToPublishSubscribeChannelPath(FileSystemParameter parameter, string connectionstring, string path, string subscription)
        {
            var hash = GetHashString(connectionstring);

            return Path.Combine(parameter.Path, "topics", hash, path, subscription);
        }

        public Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel)
        {
            var path = CreatePublishSubscribeChannelPath(_parameter,  channel.ConnectionString, channel.Path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> CreateIfNotExist(PointToPointChannelResource channel)
        {
            var path = CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel)
        {
            var path = CreatePublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(PointToPointChannelResource channel)
        {
            var path = CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<PublishSubscribeChannelStatistics> Get(PublishSubscribeChannelResource channel)
        {
            var path = CreatePublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path);

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);

                return Task.FromResult(new PublishSubscribeChannelStatistics(channel.Path) { });
            }

            return Task.FromResult(default(PublishSubscribeChannelStatistics));
        }

        public Task<PointToPointChannelStatistics> Get(PointToPointChannelResource channel)
        {
            var path = CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);

                return Task.FromResult(new PointToPointChannelStatistics(channel.Path) {});
            }

            return Task.FromResult(default(PointToPointChannelStatistics));
        }

        public Task<SubscriptionToPublishSubscribeChannelStatistics> Get(SubscriptionToPublishSubscribeChannelResource channel)
        {
            var path = CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);

                return Task.FromResult(new SubscriptionToPublishSubscribeChannelStatistics(channel.Subscription, channel.Path) { });
            }

            return Task.FromResult(default(SubscriptionToPublishSubscribeChannelStatistics));
        }
    }
}