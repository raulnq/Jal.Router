using System;
using System.Threading;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management.ShutdownWatcher
{
    public class ShutdownSignalWatcher : IShutdownWatcher
    {
        public void Start(CancellationTokenSource tokensource)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}