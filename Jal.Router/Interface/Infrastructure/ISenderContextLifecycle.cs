using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface ISenderContextLifecycle
    {
        SenderContext Remove(Channel channel);

        SenderContext AddOrGet(Channel channel);

        SenderContext Add(Channel channel);

        SenderContext Get(Channel channel);

        bool Exist(Channel channel);
    }
}