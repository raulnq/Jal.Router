namespace Jal.Router.Fluent.Interface
{
    public interface INameEndPointBuilder
    {
        IOnEndPointOptionBuilder ForMessage<TMessage>();
    }
}