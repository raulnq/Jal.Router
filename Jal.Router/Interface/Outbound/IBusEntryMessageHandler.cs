using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBusEntryMessageHandler
    {
        Task Handle(MessageContext context, Handler metadata);
    }
}