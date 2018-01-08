using System;
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
        public MessageContext Read(object message, Type contenttype)
        {
            var context = Read(message);

            context.ContentType = contenttype;

            var body = GetBody(message);

            context.ContentAsString = body;

            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            var content = serializer.Deserialize(body, contenttype);

            context.Content = content;

            return context;
        }

        public abstract object Write(MessageContext context);

        public abstract MessageContext Read(object message);

        public abstract string GetBody(object message);
    }
}