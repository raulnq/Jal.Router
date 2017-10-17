using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageBodyAdapter : IMessageBodyAdapter
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        protected AbstractMessageBodyAdapter(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public TContent Read<TContent, TMessage>(TMessage message)
        {
            var body = ReadBody(message);

            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            return serializer.Deserialize<TContent>(body);
        }

        public abstract string ReadBody<TMessage>(TMessage message);

        public abstract TMessage WriteBody<TMessage>(string body);

        public TMessage Write<TContent, TMessage>(TContent content)
        {
            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var body = serializer.Serialize(content);

            return WriteBody<TMessage>(body);
        }
    }
}