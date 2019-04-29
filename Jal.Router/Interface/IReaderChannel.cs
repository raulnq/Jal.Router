using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IReaderChannel
    {
        MessageContext Read(MessageContext context, IMessageAdapter adapter);
    }
}