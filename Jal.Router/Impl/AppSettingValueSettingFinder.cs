using System.Configuration;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class AppSettingValueSettingFinder : IValueSettingFinder
    {
        public string Find(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}