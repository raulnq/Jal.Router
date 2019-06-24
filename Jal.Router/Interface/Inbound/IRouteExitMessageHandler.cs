using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouteExitMessageHandler
    {
        Task Handle(MessageContext context, Handler metadata);
    }
}