using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Patterns
{
    public interface IDynamicRouter<in TData>
    {
        Task Send(MessageContext context, TData data, string id="");
    }
}