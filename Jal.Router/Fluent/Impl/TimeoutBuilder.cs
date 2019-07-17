using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class TimeoutBuilder : ITimeoutBuilder
    {
        private readonly Saga _saga;

        public TimeoutBuilder(Saga saga)
        {
            _saga = saga;
        }

        public void WithTimeout(int seconds)
        {
            if (seconds < 0)
            {
                throw new ArgumentNullException(nameof(seconds));
            }
            _saga.UpdateTimeout(seconds);
        }
    }
}