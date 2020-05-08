using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelListener
    {
        void Open(ListenerContext context);

        bool IsActive(ListenerContext context);

        Task Close(ListenerContext context);

        void Listen(ListenerContext context);
    }
}