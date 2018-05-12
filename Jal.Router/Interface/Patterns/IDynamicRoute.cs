using Jal.Router.Model;

namespace Jal.Router.Interface.Patterns
{
    public interface IDynamicRoute<in TData>
    {
        bool IsValid(MessageContext context, TData data);

        void Send(MessageContext context, TData data);

        string Id { get; }

        int Order { get; }
    }
}