using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBusExitMessageHandler
    {
        Task Handle(MessageContext context, Handler metadata);
    }
}