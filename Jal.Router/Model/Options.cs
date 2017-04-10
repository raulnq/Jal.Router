using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jal.Router.Model
{
    public class Options
    {
        public Options()
        {
            Headers=new Dictionary<string, string>();
            Version = "1";
        }
        public string MessageId { get; set; }

        public string Correlation { get; set; }

        public string EndPoint { get; set; }

        public string Version { get; set; }

        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }

        public IDictionary<string,string> Headers { get; set; }

    }
}
