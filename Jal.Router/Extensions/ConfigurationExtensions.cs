using Jal.Router.Impl.Management.ShutdownWatcher;
using Jal.Router.Interface.Management;

namespace Jal.Router.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseShutdownFileWatcher(this IConfiguration configuration, string filepath)
        {
            return configuration.UseShutdownWatcher<FileShutdownWatcher, ShutdownFileWatcherParameter>(new ShutdownFileWatcherParameter() { File = filepath });
        }
    }
}
