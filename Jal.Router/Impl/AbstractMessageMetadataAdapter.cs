using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageMetadataAdapter : IMessageMetadataAdapter
    {
        public const string From = "from";
        public const string SagaId = "sagaid";
        public const string Version = "version";
        public const string Origin = "origin";
        public const string RetryCount = "retrycount";
        public const string EnclosedType = "enclosedtype";

        public abstract MessageContext Create<TMessage>(TMessage message);
        public abstract TMessage Create<TMessage>(MessageContext messagecontext, TMessage message);
    }
}