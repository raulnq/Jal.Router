using System;

namespace Jal.Router.Model
{
    public class Channel
    {
        public Type ConnectionStringExtractorType { get; set; }

        public object ToConnectionStringExtractor { get; set; }

        public string ToConnectionString { get; set; }

        public string ToPath { get; set; }

        public string ToSubscription { get; set; }

        public Action ShutdownAction { get; set; }
    }
}