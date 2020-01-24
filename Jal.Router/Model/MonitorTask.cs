using System;

namespace Jal.Router.Model
{
    public class MonitorTask
    {
        public MonitorTask(Type type, int intervalinmilliseconds)
        {
            Type = type;
            IntervalInMilliSeconds = intervalinmilliseconds;
        }
        public Type Type { get; private set; }

        public int IntervalInMilliSeconds { get; private set; }
    }
}