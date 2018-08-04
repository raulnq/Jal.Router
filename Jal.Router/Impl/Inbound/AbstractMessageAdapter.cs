using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public abstract class AbstractMessageAdapter : IMessageAdapter
    {
        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        protected readonly IBus Bus;

        public const string ContentId = "contentid";

        public const string OperationId = "operationid";

        public const string ParentId = "parentid";

        public const string Tracks = "tracking";

        public const string From = "from";

        public const string SagaId = "sagaid";

        public const string Version = "version";

        public const string Origin = "origin";

        public const string RetryCount = "retrycount";

        public const string EnclosedType = "enclosedtype";

        protected AbstractMessageAdapter(IComponentFactory factory, IConfiguration configuration, IBus bus)
        {
            Factory = factory;
            Configuration = configuration;
            Bus = bus;
        }

        public object Deserialize(string content, Type type)
        {
            var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            try
            {
                return serializer.Deserialize(content, type);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TContent Deserialize<TContent>(string content)
        {
            var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            try
            {
                return serializer.Deserialize<TContent>(content);
            }
            catch (Exception)
            {
                return default(TContent);
            }
        }

        public string Serialize(object content)
        {
            var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            try
            {
                return serializer.Serialize(content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MessageContext Read(object message, Type contenttype, bool useclaimcheck)
        {
            var context = Read(message);

            context.ContentType = contenttype;

            if (useclaimcheck && !string.IsNullOrWhiteSpace(context.ContentId))
            {
                var storage = Factory.Create<IMessageStorage>(Configuration.MessageStorageType);

                context.Content = storage.Read(context.ContentId);
            }
            else
            {
                context.Content = GetContent(message);
            }

            return context;
        }

        public object Write(MessageContext context, bool useclaimcheck)
        {
            var content = context.Content;

            if(useclaimcheck)
            {
                var storage = Factory.Create<IMessageStorage>(Configuration.MessageStorageType);

                context.ContentId = Guid.NewGuid().ToString();

                storage.Write(context.ContentId, context.Content);

                context.Content = string.Empty;
            }

            var message = Write(context);

            context.Content = content;

            return message;
        }

        protected abstract object Write(MessageContext context);

        protected abstract MessageContext Read(object message);

        protected abstract string GetContent(object message);
    }
}