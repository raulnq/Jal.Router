using System;

namespace Jal.Router.Interface
{
    public interface IRetryPolicy
    {
        TimeSpan NextRetryInterval(int currentretrycount);

        bool CanRetry(int currentretrycount);
    }
}