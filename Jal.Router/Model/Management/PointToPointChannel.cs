using System;

namespace Jal.Router.Model.Management
{
    public class PointToPointChannel
    {
        public PointToPointChannel(string path)
        {
            Path = path;
        }
        public string Path { get; set; }

        public string ConnectionString { get; set; }

        public Type ConnectionStringValueFinderType { get; set; }

        public object ConnectionStringProvider { get; set; }
    }
}