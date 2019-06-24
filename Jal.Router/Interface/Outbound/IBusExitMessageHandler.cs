using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface.Outbound
{
    public interface IBusExitMessageHandler
    {
        Task Handle(MessageContext context, Handler metadata);
    }
}