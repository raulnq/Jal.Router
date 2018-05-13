using Jal.Router.Model;

namespace Jal.Router.Interface.Patterns
{
    public interface IDynamicRouter<in TData>
    {
        void Send(MessageContext context, TData data, string id="");
    }
}