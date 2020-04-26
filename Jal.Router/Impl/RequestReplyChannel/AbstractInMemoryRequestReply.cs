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

        protected AbstractInMemoryRequestReply(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider, IInMemoryTransport transport)
            : base(factory, logger)
        {
            _parameter = provider.Get<InMemoryParameter>();

            _transport = transport;
        }

        public void Open(SenderContext sendercontext)
        {
            _name = _transport.CreateName(sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);
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