using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ISenderContextLoader
    {
        SenderContext Create(Channel channel);

        void Open(SenderContext sendercontext);
    }
}