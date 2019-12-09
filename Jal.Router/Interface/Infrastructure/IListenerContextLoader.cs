using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IListenerContextLoader
    {
        void Load(Route route, Channel channel);
    }
}