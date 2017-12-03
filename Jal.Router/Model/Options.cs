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
            SagaInfo = new SagaInfo();
        }
        public string Id { get; set; }

        public SagaInfo SagaInfo { get; set; }

        public string EndPointName { get; set; }

        public string Version { get; set; }

        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }

        public IDictionary<string,string> Headers { get; set; }

        public int RetryCount { get; set; }
    }
}
