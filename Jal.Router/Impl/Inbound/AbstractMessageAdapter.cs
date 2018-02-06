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

        public const string ParentSagaIds = "parentsagaids";

        public const string Version = "version";

        public const string Origin = "origin";

        public const string ParentOrigins = "parentorigins";

        public const string RetryCount = "retrycount";

        public const string EnclosedType = "enclosedtype";

        protected AbstractMessageAdapter(IComponentFactory factory, IConfiguration configuration, IBus bus, IStorageFacade facade)
        {
            _factory = factory;
            _configuration = configuration;
            Bus = bus;
            Facade = facade;
        }

        protected object Deserialize(string content, Type type)
        {
            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            try
            {
                return serializer.Deserialize(content, type);
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string Serialize(object content)
        {
            var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

            try
            {
                return serializer.Serialize(content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MessageContext Read(object message, Type contenttype)
        {
            var context = Read(message);

            context.ContentType = contenttype;

            var body = GetBody(message);

            context.ContentAsString = body;

            context.Content = Deserialize(body, contenttype);

            return context;
        }

        public abstract object Write(MessageContext context);

        public abstract MessageContext Read(object message);

        public abstract string GetBody(object message);
    }
}