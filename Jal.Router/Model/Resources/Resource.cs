using System.Collections.Generic;

namespace Jal.Router.Model
{
    public abstract class Resource
    {
        public string Path { get; }

        public string ConnectionString { get; private set; }

        public Dictionary<string, string> Properties { get; }

        protected Resource(string path, string connectionstring, Dictionary<string, string> properties)
        {
            Path = path;

            Properties = properties;

            ConnectionString = connectionstring;
        }
    }
}