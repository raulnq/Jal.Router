using Jal.Router.Interface.Management;
using Jal.Router.Logger.Impl;
using Jal.Router.Model.Management;

namespace Jal.Router.Logger.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseCommonLogging(this IConfiguration configuration)
        {
            return configuration
                .AddOutboundMiddleware<BusLogger>()
                .AddInboundMiddleware<RouterLogger>()
                .AddLogger<BeatLogger, Beat>();
        }
    }
}