using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IRouteEntryMessageHandler
    {
        Task Handle(MessageContext context, Handler metadata);
    }
}