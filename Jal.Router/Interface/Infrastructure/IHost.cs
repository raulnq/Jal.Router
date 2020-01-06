using System;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IHost
    {
        void RunAndBlock(Action poststartedaction = null);

        void Run();

        Task Startup();

        Task Shutdown();

        IConfiguration Configuration { get; }
    }
}
