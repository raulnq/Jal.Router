using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IStartupTask
    {
        Task Run();
    }
}