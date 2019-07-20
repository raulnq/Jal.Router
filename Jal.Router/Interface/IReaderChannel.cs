using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IReaderChannel
    {
        Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter);
    }
}