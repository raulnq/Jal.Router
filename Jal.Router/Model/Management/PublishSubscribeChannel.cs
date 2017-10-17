using System;

namespace Jal.Router.Model.Management
{
    public class PublishSubscribeChannel
    {
        public PublishSubscribeChannel(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public Type ConnectionStringExtractorType { get; set; }

        public object ToConnectionStringExtractor { get; set; }
    }
}