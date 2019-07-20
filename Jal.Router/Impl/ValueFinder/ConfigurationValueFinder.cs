using Microsoft.Extensions.Configuration;

namespace Jal.Router.Impl
{
    public class ConfigurationValueFinder : Jal.Router.Interface.IValueFinder
    {
        private readonly IConfiguration _configuration;

        public ConfigurationValueFinder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Find(string name)
        {
            return _configuration[name];
        }
    }
}
