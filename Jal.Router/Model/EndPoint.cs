using System;

namespace Jal.Router.Model
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

        public string From { get; set; }

        public object ToConnectionStringExtractor { get; set; }

        public object ToPathExtractor { get; set; }

        public object ReplyToConnectionStringExtractor { get; set; }

        public object ReplyToPathExtractor { get; set; }
    }
}