﻿using Jal.Router.Impl;
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
                .SetDefaultTransportName("File System")
                .UsePointToPointChannel<FileSystemPointToPointChannel>()
                .UsePublishSubscribeChannel<FileSystemPublishSubscribeChannel>()
                .UseRequestReplyChannelFromPointToPointChannel<FileSystemRequestReplyFromPointToPointChannel>()
                .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                .UsePointToPointResource<FileSystemPointToPointlResource>()
                .UsePublishSubscribeResource<FileSystemPublishSubscribeResource>()
                .UseSubscriptionToPublishSubscribeResource<FileSystemSubscriptionToPublishSubscribeResource>()
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
                .SetDefaultTransportName("Memory")
                .UsePointToPointChannel<InMemoryPointToPointChannel>()
                .UsePublishSubscribeChannel<InMemoryPublishSubscribeChannel>()
                .UseRequestReplyChannelFromPointToPointChannel<InMemoryRequestReplyFromPointToPointChannel>()
                .UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel>()
                .UsePointToPointResource<InMemoryPointToPointResource>()
                .UsePublishSubscribeResource<InMemoryPublishSubscribeResource>()
                .UseSubscriptionToPublishSubscribeResource<InMemorySubscriptionToPublishSubscribeResource>()
                .UseMessageAdapter<MessageAdapter>()
                .AddParameter(p);
        }
    }
}
