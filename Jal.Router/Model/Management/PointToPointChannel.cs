using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Management
{
    public class PointToPointChannel
    {
        public PointToPointChannel(string path)
        {
            Path = path;

            Properties = new Dictionary<string, string>();
        }
        public string Path { get; }

        public string ConnectionString { get; set; }

        public Type ConnectionStringValueFinderType { get; set; }

        public object ConnectionStringProvider { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}