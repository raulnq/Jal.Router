using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IListenerContextLoader
    {
        void Add(Route route);

        Task Remove(Route route);
    }
}