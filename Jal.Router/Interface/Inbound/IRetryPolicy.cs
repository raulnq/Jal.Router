using System;
using System.Collections.Generic;

namespace Jal.Router.Interface.Inbound
{
    public interface IRetryPolicy
    {
        TimeSpan NextRetryInterval(int currentretrycount);

        bool CanRetry(int currentretrycount);
    }
}