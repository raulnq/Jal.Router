using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class LinearRetryPolicy : IRetryPolicy
    {
        private readonly int _seconds;

        public LinearRetryPolicy(int seconds)
        {
            _seconds = seconds;
        }

        public TimeSpan RetryInterval(int currentRetryCount)
        {
            return TimeSpan.FromSeconds(_seconds);
        }
    }
}