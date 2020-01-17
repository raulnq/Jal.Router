using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ISenderContextCreator
    {
        SenderContext Create(Channel channel);

        void Open(SenderContext sendercontext);
    }
}