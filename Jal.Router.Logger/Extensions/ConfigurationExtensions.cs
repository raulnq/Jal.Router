using Jal.Router.Interface.Management;
using Jal.Router.Logger.Impl;

namespace Jal.Router.Logger.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UsingCommonLogging(this IConfiguration configuration)
        {
            configuration.UsingBusLogger<BusLogger>();

            configuration.UsingRouterLogger<RouterLogger>();
        }
    }
}