using System;

namespace Jal.Router.Interface
{
    public interface IRetryPolicy
    {
        TimeSpan NextRetryInterval(int currentRetryCount);

        bool CanRetry(int currentRetryCount, TimeSpan nextretryinterval);
    }
}