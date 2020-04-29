using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.ApplicationInsights
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseApplicationInsights(this IConfiguration configuration)
        {
            return configuration
                .AddEndpointMiddleware<BusLogger>()
                .AddRouteMiddleware<RouterLogger>()
                .AddLogger<BeatLogger, Beat>()
                .AddLogger<StatisticsLogger, Statistic>();
        }
    }
}