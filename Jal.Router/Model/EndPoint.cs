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

        public string OriginKey { get; set; }

        public Type ExtractorType { get; set; }

        public Type MessageType { get; set; }

        public string OriginName { get; set; }

        public object ToConnectionStringExtractor { get; set; }

        public object ToPathExtractor { get; set; }

        public object ReplyToConnectionStringExtractor { get; set; }//TODO delete

        public object ReplyToPathExtractor { get; set; }//TODO delete
    }
}