using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public abstract class AbstractMessageAdapter : IMessageAdapter
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        protected readonly IBus Bus;

        protected readonly IStorageFacade Facade;

        public const string From = "from";

        public const string SagaId = "sagaid";

        public const string ParentSagaId = "parentsagaid";

        public const string Version = "version";

        public const string Origin = "origin";

        public const string RetryCount = "retrycount";

        public const string EnclosedType = "enclosedtype";

        protected AbstractMessageAdapter(IComponentFactory factory, IConfiguration configuration, IBus bus, IStorageFacade facade)
        {
            _factory = factory;
            _configuration = configuration;
            Bus = bus;
            Facade = facade;
        }
        public MessageContext Read(object message, Type contenttype)
        {
            var context = Read(message);

            context.ContentType = contenttype;

            var body = GetBody(message);

            context.ContentAsString = body;

            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            try
            {
                var content = serializer.Deserialize(body, contenttype);

                context.Content = content;
            }
            catch (Exception)
            {
                context.Content = null;
            }

            return context;
        }

        public abstract object Write(MessageContext context);

        public abstract MessageContext Read(object message);

        public abstract string GetBody(object message);
    }
}