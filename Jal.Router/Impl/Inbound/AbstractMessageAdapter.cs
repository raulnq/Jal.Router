using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
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
        public MessageContext<TContent> Read<TContent, TMessage>(TMessage message)
        {
            var context = Create(message);

            context.ContentType = typeof (TContent);

            var body = ReadBody(message);

            context.ContentAsString = body;

            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var content = serializer.Deserialize<TContent>(body);

            return new MessageContext<TContent>(context, content);
        }

        public abstract TMessage Write<TContent, TMessage>(MessageContext<TContent> context);

        public abstract MessageContext Create<TMessage>(TMessage message);

        public abstract string ReadBody<TMessage>(TMessage message);
    }
}