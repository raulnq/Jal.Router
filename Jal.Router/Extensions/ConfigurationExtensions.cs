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
                //.UseRequestReplyChannelFromPointToPointChannel<AzureServiceBusRequestReplyFromPointToPointChannel>()
                //.UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                .UseChannelResource<FileSystemChannelResource>()
                .UseMessageAdapter<FileSystemMessageAdapter>()
                .AddParameter(p);
        }
    }
}
