using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public abstract class AbstractInMemoryRequestReply : AbstractChannel
    {
        protected readonly InMemoryParameter _parameter;

        protected readonly IInMemoryTransport _transport;

        private string _name;

        protected AbstractInMemoryRequestReply(IComponentFactoryGateway factory, ILogger logger, IParameterProvider provider, IInMemoryTransport transport)
            : base(factory, logger)
        {
            _parameter = provider.Get<InMemoryParameter>();

            _transport = transport;
        }

        public void Open(SenderContext sendercontext)
        {
            _name = _transport.CreateName(sendercontext.Channel.ToConnectionString, sendercontext.Channel.ToPath);
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
                    _transport.Dispach(_name, m);
                }

                return m.Id;
            }

            return string.Empty;
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return true;
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }
    }
}