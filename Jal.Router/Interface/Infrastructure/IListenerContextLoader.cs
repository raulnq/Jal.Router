using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IListenerContextLoader
    {
        ListenerContext Load(Channel channel);
    }
}