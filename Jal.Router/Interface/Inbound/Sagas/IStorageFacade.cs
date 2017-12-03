using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface IStorageFacade
    {
        void Save<TData>(MessageContext context, TData data) where TData : class, new();
    }
}