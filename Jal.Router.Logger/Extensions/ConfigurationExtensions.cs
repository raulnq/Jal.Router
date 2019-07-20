using Jal.Router.Interface;
using Jal.Router.Logger.Impl;
using Jal.Router.Model;

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