using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ExponentialRetryPolicy : IRetryPolicy
    {
        private readonly int _seconds;

        public ExponentialRetryPolicy(int seconds)
        {
            _seconds = seconds;
        }

        public TimeSpan RetryInterval(int currentRetryCount)
        {
            return TimeSpan.FromSeconds(Math.Pow(_seconds, currentRetryCount));
        }
    }
}