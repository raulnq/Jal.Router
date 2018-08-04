using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Options
    {
        public Options()
        {
            Headers=new Dictionary<string, string>();
            Version = "1";
            RetryCount = 0;
            SagaContext = new SagaContext();
            Tracks = new List<Track>();
            Identity = new Identity();
        }
        public Identity Identity { get; set; }

        public SagaContext SagaContext { get; set; }

        public string EndPointName { get; set; }

        public string Version { get; set; }

        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }

        public IDictionary<string,string> Headers { get; set; }

        public int RetryCount { get; set; }

        public List<Track> Tracks { get; set; }
    }
}
