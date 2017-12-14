using System.Threading;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
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