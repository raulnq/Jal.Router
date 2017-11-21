using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageAdapter : IMessageAdapter
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public const string From = "from";

        public const string SagaId = "sagaid";

        public const string Version = "version";

        public const string Origin = "origin";

        public const string RetryCount = "retrycount";

        public const string EnclosedType = "enclosedtype";

        protected AbstractMessageAdapter(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }
        public InboundMessageContext<TContent> Read<TContent, TMessage>(TMessage message)
        {
            var context = Create(message);

            context.ContentType = typeof (TMessage);

            var body = ReadBody(message);

            context.Body = body;

            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var content = serializer.Deserialize<TContent>(body);

            return new InboundMessageContext<TContent>(context, content);
        }

        public abstract TMessage Write<TContent, TMessage>(OutboundMessageContext<TContent> context);

        public abstract MessageContext Create<TMessage>(TMessage message);

        public abstract string ReadBody<TMessage>(TMessage message);
    }
}