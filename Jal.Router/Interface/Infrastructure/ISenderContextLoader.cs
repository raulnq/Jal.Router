using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface ISenderContextLoader
    {
        Task Remove(EndPoint endpoint);

        void Add(EndPoint endpoint);
    }
}