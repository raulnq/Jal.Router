namespace Jal.Router.Fluent.Interface
{
    public interface IForMessageEndPointBuilder
    {
        IToEndPointBuilder ForMessage<TMessage>();
    }
}