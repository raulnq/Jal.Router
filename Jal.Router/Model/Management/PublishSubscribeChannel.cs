using System;

namespace Jal.Router.Model.Management
{
    public class PublishSubscribeChannel
    {
        public PublishSubscribeChannel(string path)
        {
            Path = path;
        }
        public string Path { get; }
        public string ConnectionString { get; set; }

        public Type ConnectionStringValueFinderType { get; set; }

        public object ConnectionStringProvider { get; set; }
    }
}