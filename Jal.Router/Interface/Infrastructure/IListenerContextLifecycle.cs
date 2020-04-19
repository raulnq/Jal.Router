using Jal.Router.Model;
namespace Jal.Router.Interface
{
    public interface IListenerContextLifecycle
    {
        ListenerContext AddOrGet(Channel channel);

        ListenerContext Add(Channel channel);

        ListenerContext Get(Channel channel);

        ListenerContext Remove(Channel channel);

        bool Exist(Channel channel);
    }
}