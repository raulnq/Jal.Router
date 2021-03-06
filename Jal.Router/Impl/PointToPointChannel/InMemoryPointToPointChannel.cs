﻿using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class InMemoryPointToPointChannel : AbstractChannel, IPointToPointChannel
    {
        private readonly IInMemoryTransport _transport;

        private readonly InMemoryParameter _parameter;

        private string _listenername;

        private string _sendername;

        public InMemoryPointToPointChannel(IParameterProvider provider, IComponentFactoryFacade factory, ILogger logger, IInMemoryTransport inmemorysystem) : base(factory, logger)
        {
            _parameter = provider.Get<InMemoryParameter>();

            _transport = inmemorysystem;
        }

        public Task<Statistic> GetStatistic(Channel channel)
        {
            return Task.FromResult(default(Statistic));
        }

        public Task<bool> DeleteIfExist(Channel channel)
        {
            return Task.FromResult(false);
        }

        public Task Close(ListenerContext listenercontext)
        {
            return Task.CompletedTask;
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CreateIfNotExist(Channel channel)
        {
            var name = _transport.CreateName(channel.ConnectionString, channel.Path);

            if (!_transport.Exists(name))
            {
                _transport.Create(name);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public bool IsActive(ListenerContext listenercontext)
        {
            return true;
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return true;
        }

        public void Listen(ListenerContext listenercontext)
        {
            _transport.Subscribe(_listenername, async message =>
            {
                var context = await listenercontext.Read(message).ConfigureAwait(false);

                Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");

                try
                {
                    await listenercontext.Dispatch(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Message {context.Id} failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} {ex}");
                }
                finally
                {
                    Logger.Log($"Message {context.Id} completed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");
                }
            });
        }

        public void Open(ListenerContext listenercontext)
        {
            _listenername = _transport.CreateName(listenercontext.Channel.ConnectionString, listenercontext.Channel.Path);
        }

        public void Open(SenderContext sendercontext)
        {
            _sendername = _transport.CreateName(sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);
        }

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var m = message as Message;

            if (m != null)
            {
                var handledbymock = false;

                if (_parameter.Handlers.ContainsKey(sendercontext.EndPoint.Name))
                {
                    await _parameter.Handlers[sendercontext.EndPoint.Name](sendercontext.MessageSerializer, m);

                    handledbymock = true;
                }

                if (!handledbymock)
                {
                    _transport.Dispach(_sendername, m);
                }

                return m.Id;
            }

            return string.Empty;
        }
    }
}