using System.Threading;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management.ShutdownWatcher
{
    public class NullShutdownWatcher : IShutdownWatcher
    {
        public void Start(CancellationTokenSource tokensource)
        {

        }

        public void Stop()
        {

        }
    }
}