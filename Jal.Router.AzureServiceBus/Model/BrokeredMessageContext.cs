using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jal.Router.AzureServiceBus.Model
{
    public class BrokeredMessageContext
    {
        public string MessageId { get; set; }
        public string CorrelationId { get; set; }
        public string ReplyTo { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
