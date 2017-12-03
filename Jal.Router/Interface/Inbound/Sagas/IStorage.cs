using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface IStorage
    {
        void Create(MessageContext context);

        void Create<TData>(MessageContext context, TData data) where TData : class, new();

        void Update<TData>(MessageContext context, TData data) where TData : class, new();

        TData Find<TData>(MessageContext context) where TData : class, new();
    }
}