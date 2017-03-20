using System;

namespace Jal.Router.AzureServiceBus.Model
{
    public class EndPoint
    {
        public EndPoint(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public Type ExtractorType { get; set; }

        public Type MessageType { get; set; }

        public object FromExtractor { get; set; }

        public object ToConnectionStringExtractor { get; set; }

        public object ToPathExtractor { get; set; }

        public object ReplyToConnectionStringExtractor { get; set; }

        public object ReplyToPathExtractor { get; set; }
    }
}