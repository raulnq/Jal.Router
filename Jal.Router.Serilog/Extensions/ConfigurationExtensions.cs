using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Serilog
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseSerilog(this IConfiguration configuration)
        {
            return configuration
                .AddOutboundMiddleware<BusLogger>()
                .AddInboundMiddleware<RouterLogger>()
                .AddLogger<BeatLogger, Beat>();
        }
    }
}