using System;
using System.Collections.Generic;
using Jal.Router.Interface;

namespace Jal.Router.Model
{
    public class RetryOptions
    {
        public RetryOptions(InboundMessageContext context, int maxretrycount, IRetryPolicy retrypolicy)
        {
            MessageId = context.Id;
            Correlation = context.Id;
            Version = context.Version;
            Origin = context.Origin;
            RetryCount = context.RetryCount;
            From = context.From;
            Headers = context.Headers;
            RetryPolicy = retrypolicy;
            MaxRetryCount = maxretrycount;
        }

        public IRetryPolicy RetryPolicy { get; }

        public int MaxRetryCount { get; }

        public int RetryCount { get; }

        public string From { get;  }

        public string MessageId { get;  }

        public string Correlation { get; }

        public string Version { get;  }

        public IDictionary<string, string> Headers { get; }

        public string Origin { get; }
    }
}