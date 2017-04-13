using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
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

        public bool CanRetry(int currentRetryCount, TimeSpan nextretryinterval)
        {
            return currentRetryCount < _maxretrycount;
        }

        public TimeSpan NextRetryInterval(int currentRetryCount)
        {
            return TimeSpan.FromSeconds(Math.Pow(_seconds, currentRetryCount));
        }
    }
}