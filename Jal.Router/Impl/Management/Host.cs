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

        private int _state;

        private readonly CancellationTokenSource _cancellationtokensource;
        public Host(IStartup startup, IMonitor monitor, IConfiguration configuration, IShutdown shutdown, IComponentFactory factory)
        {
            _startup = startup;
            _monitor = monitor;
            Configuration = configuration;
            _shutdown = shutdown;
            _factory = factory;
            _cancellationtokensource = new CancellationTokenSource();
        }

        public void Start()
        {
            Task.Run(() => StartAsync()).GetAwaiter().GetResult();
        }

        public Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Interlocked.CompareExchange(ref _state, StateStarting, StateNotStarted) != StateNotStarted)
            {
                throw new InvalidOperationException("Start has already been called");
            }

            _watcher = _factory.Create<IShutdownWatcher>(Configuration.ShutdownWatcherType);

            _startup.Start();

            Console.WriteLine("Host Started");

            _monitor.Start();

            _watcher.Start(_cancellationtokensource);

            _state = StateStarted;

            return Task.FromResult(0);
        }

        public void Stop()
        {
            Task.Run(() => StopAsync()).GetAwaiter().GetResult();
        }

        public void RunAndBlock()
        {
            Start();

            _cancellationtokensource.Token.WaitHandle.WaitOne();

            Stop();
        }

        public IConfiguration Configuration { get; }

        public Task StopAsync()
        {
            Interlocked.CompareExchange(ref _state, StateStoppingOrStopped, StateStarted);

            if (_state != StateStoppingOrStopped)
            {
                throw new InvalidOperationException("The host has not yet starteds");
            }

            _shutdown.Stop();

            _watcher.Stop();

            _cancellationtokensource.Dispose();

            Console.WriteLine("Host Stopped");

            return Task.FromResult(0);
        }
    }
}
