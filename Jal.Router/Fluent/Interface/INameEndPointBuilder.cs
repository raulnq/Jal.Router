namespace Jal.Router.Fluent.Interface
{
    public interface INameEndPointBuilder
    {
        IFromEndPointBuilder ForMessage<TMessage>();
    }
}