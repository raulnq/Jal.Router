using Jal.Router.Interface;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Management
{
    public class PointToPointChannel
    {
        public PointToPointChannel(string path, string connectionstring, Dictionary<string, string> properties)
        {
            Path = path;

            Properties = properties;

            ConnectionString = connectionstring;
        }

        public PointToPointChannel(string path, Dictionary<string, string> properties, Type type, Func<IValueFinder, string> provider)
        {
            Path = path;

            Properties = properties;

            ConnectionStringValueFinderType = type;

            ConnectionStringProvider = provider;
        }
        public string Path { get; }

        public string ConnectionString { get; set; }

        public Type ConnectionStringValueFinderType { get; }

        public Func<IValueFinder, string> ConnectionStringProvider { get; }

        public Dictionary<string, string> Properties { get; }
    }
}