using Jal.Router.Interface;
using System.Threading;

namespace Jal.Router.Impl
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