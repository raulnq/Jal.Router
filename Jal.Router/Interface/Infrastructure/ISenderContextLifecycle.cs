using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ISenderContextLifecycle
    {
        SenderContext Remove(Channel channel);

        SenderContext Add(EndPoint endpoint, Channel channel);

        SenderContext Get(Channel channel);

        bool Exist(Channel channel);
    }
}