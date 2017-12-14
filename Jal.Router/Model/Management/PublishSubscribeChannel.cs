using System;

namespace Jal.Router.Model.Management
{
    public class PublishSubscribeChannel
    {
        public PublishSubscribeChannel(string path)
        {
            Path = path;
        }
        public string Path { get; set; }

        public Type ConnectionStringExtractorType { get; set; }

        public object ToConnectionStringExtractor { get; set; }
    }
}