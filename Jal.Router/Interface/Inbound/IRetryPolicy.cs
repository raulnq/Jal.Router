using System;

namespace Jal.Router.Interface.Inbound
{
    public interface IRetryPolicy
    {
        TimeSpan NextRetryInterval(int currentretrycount);

        bool CanRetry(int currentretrycount, TimeSpan nextretryinterval);
    }
}