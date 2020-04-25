using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseMemoryAsStorage(this IConfiguration configuration)
        {
            return configuration
                .UseEntityStorage<InMemoryEntityStorage>();
        }

        public static IConfiguration AddShutdownFileWatcher(this IConfiguration configuration, string filepath)
        {
            return configuration.AddShutdownWatcher<FileShutdownWatcher, ShutdownFileWatcherParameter>(new ShutdownFileWatcherParameter() { File = filepath });
        }

        public static IConfiguration UseFileSystemAsTransport(this IConfiguration configuration, FileSystemParameter parameter = null)
        {
            var p = new FileSystemParameter();

            if (parameter != null)
            {
                p = parameter;
            }

            return configuration
                .SetTransportName("File System")
                .UsePointToPointChannel<FileSystemPointToPointChannel>()
                .UsePublishSubscribeChannel<FileSystemPublishSubscribeChannel>()
                .UseRequestReplyChannelFromPointToPointChannel<FileSystemRequestReplyFromPointToPointChannel>()
                .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                .UsePointToPointResourceManager<FileSystemPointToPointlResourceManager>()
                .UsePublishSubscribeResourceManager<FileSystemPublishSubscribeResourceManager>()
                .UseSubscriptionToPublishSubscribeResourceManager<FileSystemSubscriptionToPublishSubscribeResourceManager>()
                .UseMessageAdapter<MessageAdapter>()
                .AddParameter(p);
        }

        public static IConfiguration UseMemoryAsTransport(this IConfiguration configuration, InMemoryParameter parameter = null)
        {
            var p = new InMemoryParameter();

            if (parameter != null)
            {
                p = parameter;
            }

            return configuration
                .SetTransportName("Memory")
                .UsePointToPointChannel<InMemoryPointToPointChannel>()
                .UsePublishSubscribeChannel<InMemoryPublishSubscribeChannel>()
                .UseRequestReplyChannelFromPointToPointChannel<InMemoryRequestReplyFromPointToPointChannel>()
                .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                .UsePointToPointResourceManager<InMemoryPointToPointResourceManager>()
                .UsePublishSubscribeResourceManager<InMemoryPublishSubscribeResourceManager>()
                .UseSubscriptionToPublishSubscribeResourceManager<InMemorySubscriptionToPublishSubscribeResourceManager>()
                .UseMessageAdapter<MessageAdapter>()
                .AddParameter(p);
        }
    }
}
