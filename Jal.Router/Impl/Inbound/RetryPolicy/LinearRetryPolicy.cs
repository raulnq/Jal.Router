using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class LinearRetryPolicy : IRetryPolicy
    {
        private readonly int _seconds;

        private readonly int _maxretrycount;

        public LinearRetryPolicy(int seconds, int maxretrycount)
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
            return TimeSpan.FromSeconds(_seconds);
        }
    }
}