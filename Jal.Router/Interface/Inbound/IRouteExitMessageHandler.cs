using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IRouteExitMessageHandler
    {
        Task Handle(MessageContext context, Handler metadata);
    }
}