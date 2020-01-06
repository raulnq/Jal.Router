using Jal.Router.Interface;
using Jal.Router.Model;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class InMemoryChannelResource : IChannelResource
    {
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

        public static string CreatePointToPointChannelPath(string connectionstring, string path)
        {
            var hash = GetHashString(connectionstring);

            return $"queue-{hash}-{path}";
        }

        public static string CreatePublishSubscribeChannelPath(string connectionstring, string path)
        {
            var hash = GetHashString(connectionstring);

            return $"topic-{hash}-{path}";
        }

        public static string CreateSubscriptionToPublishSubscribeChannelPath(FileSystemParameter parameter, string connectionstring, string path, string subscription)
        {
            var hash = GetHashString(connectionstring);

            return $"topic-{hash}-{path}";
        }

        public Task<bool> CreateIfNotExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            return Task.FromResult(false);
        }

        public Task<bool> CreateIfNotExist(PublishSubscribeChannelResource channel)
        {
            var name = CreatePublishSubscribeChannelPath(channel.ConnectionString, channel.Path);

            if (!PostalOffice.Exists(name))
            {
                PostalOffice.Create(name);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> CreateIfNotExist(PointToPointChannelResource channel)
        {
            var name = CreatePointToPointChannelPath(channel.ConnectionString, channel.Path);

            if (!PostalOffice.Exists(name))
            {
                PostalOffice.Create(name);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(SubscriptionToPublishSubscribeChannelResource channel)
        {
            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(PublishSubscribeChannelResource channel)
        {
            var name = CreatePublishSubscribeChannelPath(channel.ConnectionString, channel.Path);

            if (PostalOffice.Exists(name))
            {
                PostalOffice.Delete(name);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> DeleteIfExist(PointToPointChannelResource channel)
        {
            var name = CreatePointToPointChannelPath(channel.ConnectionString, channel.Path);

            if (PostalOffice.Exists(name))
            {
                PostalOffice.Delete(name);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
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