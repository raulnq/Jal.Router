using System.Configuration;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ConnectionStringValueSettingFinder : IValueSettingFinder
    {
        public string Find(string name)
        {
            var section = ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;

            if (section != null) return section.ConnectionStrings[name].ConnectionString;

            return string.Empty;
        }
    }
}