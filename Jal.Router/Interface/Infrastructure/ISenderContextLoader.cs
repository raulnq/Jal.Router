using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ISenderContextLoader
    {
        void Load(EndPoint endpoint, Channel channel);
    }
}