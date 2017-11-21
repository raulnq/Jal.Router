using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface.Management;

namespace Jal.Router.ApplicationInsights.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UsingApplicationInsights(this IConfiguration configuration)
        {
            configuration.AddOutboundMiddleware<ApplicationInsightsBusLogger>();

            configuration.AddInboundMiddleware<ApplicationInsightsRouterLogger>();
        }
    }
}