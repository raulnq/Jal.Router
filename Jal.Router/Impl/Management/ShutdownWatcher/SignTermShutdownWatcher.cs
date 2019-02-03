using System;
using System.Runtime.Loader;
using System.Threading;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management.ShutdownWatcher
{
    public class SignTermShutdownWatcher : IShutdownWatcher
    {
        private readonly ILogger _log;

        public SignTermShutdownWatcher(ILogger log)
        {
            _log = log;
        }
        public void Start(CancellationTokenSource tokensource)
        {
            AssemblyLoadContext.Default.Unloading += OnUnloading;

            void OnUnloading(AssemblyLoadContext obj)
            {
                if(tokensource!=null && !tokensource.IsCancellationRequested)
                {
                    tokensource.Cancel();
                }
                
            }

            _log.Log("Listen SIGTERM signal to shut down");
        }

        public void Stop()
        {

        }
    }
}