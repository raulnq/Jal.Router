using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelSender
    {
        void Open(SenderContext context);

        Task<string> Send(SenderContext context, object message);

        bool IsActive(SenderContext context);

        Task Close(SenderContext context);
    }
}