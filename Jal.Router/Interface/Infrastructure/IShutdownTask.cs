using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IShutdownTask
    {
        Task Run();
    }
}