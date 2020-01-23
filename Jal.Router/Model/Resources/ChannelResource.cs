using Jal.Router.Interface;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public abstract class ChannelResource
    {
        public string Path { get; }

        public string ConnectionString { get; private set; }

        public Dictionary<string, string> Properties { get; }

        protected ChannelResource(string path, string connectionstring, Dictionary<string, string> properties)
        {
            Path = path;

            Properties = properties;

            ConnectionString = connectionstring;
        }
    }
}