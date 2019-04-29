using System.Threading.Tasks;

namespace Jal.Router.Interface.Management
{
    public interface IShutdown
    {
        Task Stop();
    }
}