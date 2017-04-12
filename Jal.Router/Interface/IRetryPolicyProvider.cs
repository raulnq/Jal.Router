namespace Jal.Router.Interface
{
    public interface IRetryPolicyProvider
    {
        IRetryPolicy Provide<TContent>();
    }
}