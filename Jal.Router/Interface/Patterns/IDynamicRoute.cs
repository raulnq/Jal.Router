using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Patterns
{
    public interface IDynamicRoute<in TData>
    {
        bool IsValid(MessageContext context, TData data);

        Task Send(MessageContext context, TData data);

        string Id { get; }

        int Order { get; }
    }
}