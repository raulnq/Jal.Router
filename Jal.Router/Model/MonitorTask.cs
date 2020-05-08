using System;

namespace Jal.Router.Model
{
    public class MonitorTask
    {
        public MonitorTask(Type type, int intervalinmilliseconds, bool runimmediately)
        {
            Type = type;
            IntervalInMilliSeconds = intervalinmilliseconds;
            RunImmediately = runimmediately;
        }
        public Type Type { get; private set; }

        public int IntervalInMilliSeconds { get; private set; }

        public bool RunImmediately { get; set; } = false;
    }
}