namespace Jal.Router.Fluent.Interface
{
    public interface INameRetryPolicyBuilder
    {
        IPolicyRetryBuilder ForMessage<TMessage>();
    }
}