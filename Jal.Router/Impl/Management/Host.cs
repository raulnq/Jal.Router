using System;
using System.Threading;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Host : IHost
    {
        private const int StateNotStarted = 0;

        private const int StateStarting = 1;

        private const int StateStarted = 2;

        private const int StateStopping = 3;

        private const int StateStopped = 4;

        private readonly IStartup _startup;

        private readonly IMonitor _monitor;

        private readonly IShutdown _shutdown;

        private IShutdownWatcher _watcher;

        private readonly IComponentFactory _factory;

        private readonly ILogger _logger;

        private int _state;

        private readonly CancellationTokenSource _cancellationtokensource;
        public Host(IStartup startup, IMonitor monitor, IConfiguration configuration, IShutdown shutdown, IComponentFactory factory, ILogger logger)
        {
            _startup = startup;
            _monitor = monitor;
            Configuration = configuration;
            _shutdown = shutdown;
            _factory = factory;
            _cancellationtokensource = new CancellationTokenSource();
            _logger = logger;
        }

        public void Startup()
        {
            _startup.Start();
        }

        private void Watch()
        {
            _watcher = _factory.Create<IShutdownWatcher>(Configuration.ShutdownWatcherType);

            _watcher.Start(_cancellationtokensource);
        }

        private void UnWatch()
        {
            _watcher.Stop();
        }

        private void Monitor()
        {
            _monitor.Start();
        }

        public void Shutdown()
        {
            _shutdown.Stop();
        }

        private void Stop()
        {
            Interlocked.CompareExchange(ref _state, StateStopping, StateStarted);

            if (_state != StateStopping)
            {
                throw new InvalidOperationException("The host is not running");
            }

            Shutdown();

            _state = StateStopped;
        }

        public void RunAndBlock()
        {
            _logger.Log($"Starting Host");

            Run();

            Watch();

            _logger.Log($"Host started");

            _cancellationtokensource.Token.WaitHandle.WaitOne();

            _logger.Log($"Stopping Host");

            UnWatch();

            Stop();

            _cancellationtokensource.Dispose();

            _logger.Log($"Host stopped");
        }

        public void Run()
        {
            if (Interlocked.CompareExchange(ref _state, StateStarting, StateNotStarted) != StateNotStarted)
            {
                throw new InvalidOperationException("Run has already been called");
            }

            Startup();

            Monitor();

            _state = StateStarted;
        }

        public IConfiguration Configuration { get; }
    }
}
