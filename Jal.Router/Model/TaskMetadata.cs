using System;

namespace Jal.Router.Model
{
    public class TaskMetadata
    {
        public TaskMetadata(Type type, int interval)
        {
            Type = type;
            Interval = interval;
        }
        public Type Type { get; private set; }

        public int Interval { get; private set; }
    }
}