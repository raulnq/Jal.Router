using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemPublishSubscribeChannel : AbstractChannel, IPublishSubscribeChannel
    {
        private readonly FileSystemParameter _parameter;

        private readonly IFileSystemTransport _transport;

        private string _path;

        public FileSystemPublishSubscribeChannel(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider, IFileSystemTransport transport)
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

            path = _transport.CreatePublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CreateIfNotExist(Channel channel)
        {
            var path = string.Empty;

            path = _transport.CreatePublishSubscribeChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return true;
        }


        public void Open(SenderContext sendercontext)
        {
            _path = _transport.CreatePublishSubscribeChannelPath(_parameter, sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);
        }

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            Thread.Sleep(500);

            var path = string.Empty;

            if (sendercontext.Channel.ReplyChannel.ChannelType == ChannelType.PointToPoint)
            {
                path = _transport.CreatePointToPointChannelPath(_parameter, sendercontext.Channel.ReplyChannel.ConnectionString, sendercontext.Channel.ReplyChannel.Path);
            }
            else
            {
                path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, sendercontext.Channel.ReplyChannel.ConnectionString, sendercontext.Channel.ReplyChannel.Path, sendercontext.Channel.ReplyChannel.Subscription);
            }

            var message = _transport.ReadFile(path, sendercontext.MessageSerializer);

            return adapter.ReadFromPhysicalMessage(message, sendercontext);
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var m = message as Message;

            if (m != null)
            {
                var filename = $"{Guid.NewGuid().ToString()}.jal";

                var handledbymock = false;

                if (_parameter.Handlers.ContainsKey(sendercontext.EndPoint.Name))
                {
                    await _parameter.Handlers[sendercontext.EndPoint.Name](sendercontext.MessageSerializer, m);

                    handledbymock = true;
                }

                if (!handledbymock)
                {
                    var folders = _transport.GetDirectories(_path);

                    foreach (var folder in folders)
                    {
                        var fullpath = Path.Combine(_path, folder);

                        _transport.CreateFile(fullpath, filename, m, sendercontext.MessageSerializer);
                    }
                }

                return m.Id;
            }

            return string.Empty;
        }
    }
}