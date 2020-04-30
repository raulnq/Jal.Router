using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class InMemoryPublishSubscribeChannel : AbstractChannel, IPublishSubscribeChannel
    {
        private readonly IInMemoryTransport _transport;

        private readonly InMemoryParameter _parameter;

        private string _sendername;

        public InMemoryPublishSubscribeChannel(IParameterProvider provider, IComponentFactoryFacade factory, ILogger logger, IInMemoryTransport inmemorysystem) : base(factory, logger)
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

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            throw new NotImplementedException();
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

        public bool IsActive(SenderContext sendercontext)
        {
            return true;
        }

        public void Open(SenderContext sendercontext)
        {
            _sendername = _transport.CreateName(sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);
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