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

        public static IConfiguration UseFileSystem(this IConfiguration configuration, FileSystemParameter parameter = null)
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

        public static IConfiguration UseInMemory(this IConfiguration configuration)
        {
            return configuration
                .SetChannelProviderName("In-Memory")
                .UsePointToPointChannel<InMemoryPointToPointChannel>()
                .UsePublishSubscribeChannel<FileSystemPublishSubscribeChannel>()
                //.UseRequestReplyChannelFromPointToPointChannel<AzureServiceBusRequestReplyFromPointToPointChannel>()
                //.UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                .UseChannelResource<InMemoryChannelResource>()
                .UseMessageAdapter<MessageAdapter>();
        }
    }
}
