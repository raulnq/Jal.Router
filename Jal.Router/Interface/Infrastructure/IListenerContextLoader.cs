using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IListenerContextLoader
    {
        ListenerContext Create(Channel channel);

        void Open(ListenerContext listenercontext);
    }
}