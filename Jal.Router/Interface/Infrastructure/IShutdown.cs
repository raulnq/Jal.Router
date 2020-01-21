using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IShutdown
    {
        Task Run();
    }
}