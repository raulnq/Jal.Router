using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ISenderContextLoader
    {
        SenderContext Load(Channel channel);
    }
}