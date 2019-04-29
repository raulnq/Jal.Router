using System.Threading.Tasks;

namespace Jal.Router.Interface.Management
{
    public interface IStartupTask
    {
        Task Run();
    }
}