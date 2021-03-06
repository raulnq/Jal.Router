﻿using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemPointToPointChannel : AbstractChannel, IPointToPointChannel
    {
        private readonly FileSystemParameter _parameter;

        private readonly IFileSystemTransport _transport;

        private FileSystemWatcher _watcher;

        private string _path;

        public FileSystemPointToPointChannel(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider, IFileSystemTransport transport)
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
            var path = _transport.CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.DeleteDirectory(path));
        }

        public Task Close(ListenerContext listenercontext)
        {
            _watcher.Dispose();

            return Task.CompletedTask;
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CreateIfNotExist(Channel channel)
        {
            var path = _transport.CreatePointToPointChannelPath(_parameter, channel.ConnectionString, channel.Path);

            return Task.FromResult(_transport.CreateDirectory(path));
        }

        public bool IsActive(ListenerContext listenercontext)
        {
            return _watcher.EnableRaisingEvents;
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return true;
        }

        public void Listen(ListenerContext listenercontext)
        {
            _watcher.Created += async (object sender, FileSystemEventArgs e) =>
            {
                if(e.FullPath.Contains(".jal"))
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
            var path = _transport.CreatePointToPointChannelPath(_parameter, listenercontext.Channel.ConnectionString, listenercontext.Channel.Path);

            _watcher = new FileSystemWatcher(path);
        }

        public void Open(SenderContext sendercontext)
        {
            _path = _transport.CreatePointToPointChannelPath(_parameter, sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);
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
                    _transport.CreateFile(_path, filename, m, sendercontext.MessageSerializer);
                }

                return m.Id;
            }

            return string.Empty;
        }
    }
}