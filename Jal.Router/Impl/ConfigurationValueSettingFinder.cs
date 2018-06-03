#if NETSTANDARD2_0
using Microsoft.Extensions.Configuration;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ConfigurationValueSettingFinder : IValueSettingFinder
    {
        private readonly IConfiguration _configuration;

        public ConfigurationValueSettingFinder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Find(string name)
        {
            return _configuration[name];
        }
    }
}
#endif