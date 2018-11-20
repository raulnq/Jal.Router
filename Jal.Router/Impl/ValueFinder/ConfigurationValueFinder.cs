using Microsoft.Extensions.Configuration;
using Jal.Router.Interface;

namespace Jal.Router.Impl.ValueFinder
{
    public class ConfigurationValueFinder : IValueFinder
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
