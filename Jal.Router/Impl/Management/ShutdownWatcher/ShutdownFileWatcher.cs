using System;
using System.IO;
using System.Threading;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management.ShutdownWatcher
{
    public class ShutdownFileWatcherParameter
    {
        public string File { get; set; }
    }

    public class ShutdownFileWatcher : IShutdownWatcher
    {
        private FileSystemWatcher _watcher;

        private readonly ShutdownFileWatcherParameter _parameter;

        private readonly ILogger _log;

        public ShutdownFileWatcher(IParameterProvider provider, ILogger log)
        {
            _parameter = provider.Get<ShutdownFileWatcherParameter>();
            _log = log;
        }

        public void Start(CancellationTokenSource tokensource)
        {
            _log.Log($"Starting shutdown file watcher, path: {_parameter.File}");

            if (string.IsNullOrWhiteSpace(_parameter.File))
            {
                return;
            }

            var directoryname = Path.GetDirectoryName(_parameter.File);

            if (directoryname == null)
            {
                return;
            }

            try
            {
                _watcher = new FileSystemWatcher(directoryname);
            }
            catch (Exception ex)
            {
                _log.Log($"Exception on the shutdown file watcher: {ex}");

                throw;
            }

            FileSystemEventHandler onchange = (o, e) =>
            {
                if (!string.IsNullOrWhiteSpace(_parameter.File))
                {
                    if (e.FullPath.IndexOf(Path.GetFileName(_parameter.File), StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        tokensource?.Cancel();
                    }
                }
            };

            _watcher.Created += onchange;
            _watcher.Changed += onchange;
            _watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;

            _log.Log("Shutdown file watcher started");
        }

        public void Stop()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
                _watcher = null;
            }
        }
    }
}