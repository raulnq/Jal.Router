namespace Jal.Router.Fluent.Interface
{
    public interface INameEndPointBuilder
    {
        IToEndPointBuilder ForMessage<TMessage>();
    }
}