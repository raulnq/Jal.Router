using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemSubscriptionToPublishSubscribeChannel : AbstractChannel, ISubscriptionToPublishSubscribeChannel
    {
        private readonly FileSystemParameter _parameter;

        private readonly IFileSystemTransport _transport;

        private FileSystemWatcher _watcher;

        public FileSystemSubscriptionToPublishSubscribeChannel(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider, IFileSystemTransport transport)
        : base(factory, logger)
        {
            _parameter = provider.Get<FileSystemParameter>();

            _transport = transport;
        }

        public Task<Statistic> GetStatistic(Channel channel)
        {
            return Task.FromResult(default(Statistic));
        }

        public Task<bool> DeleteIfExist(Channel channel)
        {
            var path = string.Empty;

            path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }

        public Task Close(ListenerContext listenercontext)
        {
            _watcher.Dispose();

            return Task.CompletedTask;
        }

        public Task<bool> CreateIfNotExist(Channel channel)
        {
            var path = string.Empty;

            path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path, channel.Subscription);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public bool IsActive(ListenerContext listenercontext)
        {
            return _watcher.EnableRaisingEvents;
        }

        public void Listen(ListenerContext listenercontext)
        {
            _watcher.Created += async (object sender, FileSystemEventArgs e) =>
            {
                if (e.FullPath.Contains(".jal"))
                {
                    Thread.Sleep(500);

                    var message = _transport.ReadFile(e.FullPath, listenercontext.MessageSerializer);

                    var context = await listenercontext.Read(message).ConfigureAwait(false);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");

                    try
                    {
                        await listenercontext.Dispatch(context).ConfigureAwait(false);

                        _transport.DeleteFile(e.FullPath);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.Id} failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} {ex}");
                    }
                    finally
                    {
                        Logger.Log($"Message {context.Id} completed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");
                    }
                }
            };

            _watcher.EnableRaisingEvents = true;
        }

        public void Open(ListenerContext listenercontext)
        {
            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, listenercontext.Channel.ConnectionString, listenercontext.Channel.Path, listenercontext.Channel.Subscription);

            _watcher = new FileSystemWatcher(path);
        }
    }
}