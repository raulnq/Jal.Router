using System;
using Jal.Router.Interface.Inbound;

namespace Jal.Router.Impl.Inbound.RetryPolicy
{

    public class ExponentialRetryPolicy : IRetryPolicy
    {
        private readonly int _seconds;

        private readonly int _maxretrycount;

        public ExponentialRetryPolicy(int seconds, int maxretrycount)
        {
            _seconds = seconds;

            _maxretrycount = maxretrycount;
        }

        public bool CanRetry(int currentretrycount)
        {
            return currentretrycount <= _maxretrycount;
        }

        public TimeSpan NextRetryInterval(int currentretrycount)
        {
            return TimeSpan.FromSeconds(Math.Pow(_seconds, currentretrycount));
        }
    }
}