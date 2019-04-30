using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IReaderChannel
    {
        Task<MessageContext> Read(MessageContext context, IMessageAdapter adapter);
    }
}