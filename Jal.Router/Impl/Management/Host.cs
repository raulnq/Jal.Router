using System;
using System.Threading;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Host : IHost
    {
        private const int StateNotStarted = 0;

        private const int StateStarting = 1;

        private const int StateStarted = 2;

        private const int StateStoppingOrStopped = 3;

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

        private void Start()
        {
            Task.Run(() => StartAsync()).GetAwaiter().GetResult();
        }

        public void Startup()
        {
            _startup.Start();
        }

        public void Shutdown()
        {
            _shutdown.Stop();
        }

        private Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Interlocked.CompareExchange(ref _state, StateStarting, StateNotStarted) != StateNotStarted)
            {
                throw new InvalidOperationException("Start has already been called");
            }

            _logger.Log($"Starting {Configuration.ApplicationName}");

            _watcher = _factory.Create<IShutdownWatcher>(Configuration.ShutdownWatcherType);

            Startup();

            _monitor.Start();

            _watcher.Start(_cancellationtokensource);

            _state = StateStarted;

            return Task.FromResult(0);
        }

        private void Stop()
        {
            Task.Run(StopAsync).GetAwaiter().GetResult();
        }

        public void RunAndBlock()
        {
            Start();

            _cancellationtokensource.Token.WaitHandle.WaitOne();

            Stop();
        }

        public void Run()
        {
            Start();
        }

        public IConfiguration Configuration { get; }

        private Task StopAsync()
        {
            Interlocked.CompareExchange(ref _state, StateStoppingOrStopped, StateStarted);

            if (_state != StateStoppingOrStopped)
            {
                throw new InvalidOperationException("The host has not yet started");
            }

            _logger.Log($"Stopping {Configuration.ApplicationName}");

            Shutdown();

            _watcher.Stop();

            _cancellationtokensource.Dispose();

            return Task.FromResult(0);
        }
    }
}
