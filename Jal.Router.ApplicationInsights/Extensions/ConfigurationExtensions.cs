using Jal.Router.Interface.Management;
using Jal.Router.Logger.Impl;

namespace Jal.Router.Logger.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UsingApplicationInsights(this IConfiguration configuration)
        {
            configuration.AddBusLogger<ApplicationInsightsBusLogger>();

            configuration.AddRouterLogger<ApplicationInsightsRouterLogger>();
        }
    }
}