using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IListenerContextCreator
    {
        ListenerContext Create(Channel channel);

        void Open(ListenerContext listenercontext);
    }
}