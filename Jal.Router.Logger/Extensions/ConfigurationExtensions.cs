using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Logger
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseCommonLogging(this IConfiguration configuration)
        {
            return configuration
                .AddEndpointMiddleware<BusLogger>()
                .AddRouteMiddleware<RouterLogger>()
                .AddLogger<BeatLogger, Beat>();
        }
    }
}