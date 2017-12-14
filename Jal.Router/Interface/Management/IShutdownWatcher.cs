using System.Threading;

namespace Jal.Router.Interface.Management
{
    public interface IShutdownWatcher
    {
        void Start(CancellationTokenSource tokensource);

        void Stop();
    }
}