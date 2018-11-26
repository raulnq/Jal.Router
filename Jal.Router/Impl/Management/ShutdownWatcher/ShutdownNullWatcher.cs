using System.Threading;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management.ShutdownWatcher
{
    public class ShutdownNullWatcher : IShutdownWatcher
    {
        public void Start(CancellationTokenSource tokensource)
        {

        }

        public void Stop()
        {

        }
    }
}