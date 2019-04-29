using System.Threading.Tasks;

namespace Jal.Router.Interface.Management
{
    public interface IShutdownTask
    {
        Task Run();
    }
}