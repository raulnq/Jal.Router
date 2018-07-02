using System;
using Jal.Router.Interface.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class FixedRetryPolicy : IRetryPolicy
    {
        private readonly int[] _intervals;

        public FixedRetryPolicy(int[] intervals)
        {
            _intervals = intervals;
        }

        public bool CanRetry(int currentretrycount, TimeSpan nextretryinterval)
        {
            return currentretrycount < _intervals.Length;
        }

        public TimeSpan NextRetryInterval(int currentretrycount)
        {
            if(currentretrycount < _intervals.Length)
            {
                return TimeSpan.FromSeconds(_intervals[currentretrycount]);
            }
            else
            {
                return TimeSpan.FromSeconds(0);
            }       
        }
    }
}