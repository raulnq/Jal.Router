using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface IStorage
    {
        void Create(MessageContext context);

        void Create(MessageContext context, object data);

        void Update(MessageContext context, object data);

        object Find(MessageContext context);
    }
}