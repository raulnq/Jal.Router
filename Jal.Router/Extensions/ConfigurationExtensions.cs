using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class ConfigurationExtensions
    {
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
                .SetChannelProviderName("File System")
                .UsePointToPointChannel<FileSystemPointToPointChannel>()
                .UsePublishSubscribeChannel<FileSystemPublishSubscribeChannel>()
                .UseRequestReplyChannelFromPointToPointChannel<FileSystemRequestReplyFromPointToPointChannel>()
                .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                .UseChannelResource<FileSystemChannelResource>()
                .UseMessageAdapter<MessageAdapter>()
                .AddParameter(p);
        }

        public static IConfiguration UseInMemoryAsTransport(this IConfiguration configuration, InMemoryParameter parameter = null)
        {
            var p = new InMemoryParameter();

            if (parameter != null)
            {
                p = parameter;
            }

            return configuration
                .SetChannelProviderName("Memory")
                .UsePointToPointChannel<InMemoryPointToPointChannel>()
                .UsePublishSubscribeChannel<InMemoryPublishSubscribeChannel>()
                .UseRequestReplyChannelFromPointToPointChannel<InMemoryRequestReplyFromPointToPointChannel>()
                .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                .UseChannelResource<InMemoryChannelResource>()
                .UseMessageAdapter<MessageAdapter>()
                .AddParameter(p);
        }
    }
}
