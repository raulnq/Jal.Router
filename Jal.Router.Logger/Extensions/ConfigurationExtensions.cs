using Jal.Router.Interface.Management;
using Jal.Router.Logger.Impl;
using Jal.Router.Model.Management;

namespace Jal.Router.Logger.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UsingCommonLogging(this IConfiguration configuration)
        {
            configuration.AddOutboundMiddleware<BusLogger>();

            configuration.AddInboundMiddleware<RouterLogger>();

            configuration.AddLogger<HeartBeatLogger, HeartBeat>();
        }
    }
}