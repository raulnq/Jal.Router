using System;

namespace Jal.Router.Interface
{
    public interface IRetryPolicy
    {
        TimeSpan RetryInterval(int currentRetryCount);
    }
}