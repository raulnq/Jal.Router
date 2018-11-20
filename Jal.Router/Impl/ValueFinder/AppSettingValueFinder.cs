using System.Configuration;
using Jal.Router.Interface;

namespace Jal.Router.Impl.ValueFinder
{
    public class AppSettingValueFinder : IValueFinder
    {
        public string Find(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}