using Jal.Router.Interface.Management;
using Jal.Router.Newtonsoft.Impl;

namespace Jal.Router.Newtonsoft.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseNewtonsoft(this IConfiguration configuration)
        {

            return configuration
                .UseMessageSerializer<JsonMessageSerializer>();
        }


    }
}