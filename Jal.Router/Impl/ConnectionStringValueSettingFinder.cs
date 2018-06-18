using System.Configuration;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ConnectionStringValueSettingFinder : IValueSettingFinder
    {
        public string Find(string name)
        {
            if (ConfigurationManager.GetSection("connectionStrings") is ConnectionStringsSection section) return section.ConnectionStrings[name].ConnectionString;

            return string.Empty;
        }
    }
}