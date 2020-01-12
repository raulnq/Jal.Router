using Jal.Router.Interface;
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

        public InMemoryPointToPointChannel(IParameterProvider provider, IComponentFactoryGateway factory, ILogger logger, IInMemoryTransport inmemorysystem) : base(factory, logger)
        {
            _parameter = provider.Get<InMemoryParameter>();

            _transport = inmemorysystem;
        }

        public Task Close(ListenerContext listenercontext)
        {
            return Task.CompletedTask;
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
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
            var adapter = Factory.CreateMessageAdapter();

            _transport.Subscribe(_listenername, async message =>
            {
                var context = adapter.ReadMetadataFromPhysicalMessage(message);

                Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");

                try
                {
                    var handlers = new List<Task>();

                    foreach (var runtimehandler in listenercontext.Routes.Select(x => x.Consumer))
                    {
                        handlers.Add(runtimehandler(message, listenercontext.Channel));
                    }

                    await Task.WhenAll(handlers.ToArray());
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
            _listenername = _transport.CreateName(listenercontext.Channel.ToConnectionString, listenercontext.Channel.ToPath);
        }

        public void Open(SenderContext sendercontext)
        {
            _sendername = _transport.CreateName(sendercontext.Channel.ToConnectionString, sendercontext.Channel.ToPath);
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var m = message as Message;

            if (m != null)
            {
                var handledbymock = false;

                foreach (var endpoint in sendercontext.Endpoints)
                {
                    if (_parameter.Handlers.ContainsKey(endpoint.Name))
                    {
                        var serializer = Factory.CreateMessageSerializer();

                        await _parameter.Handlers[endpoint.Name](serializer, m);

                        handledbymock = true;
                    }
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