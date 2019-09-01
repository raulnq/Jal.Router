namespace Jal.Router.Fluent.Interface
{
    public interface ILastNameRouteBuilder<out TData>
    {
        IHandlerBuilder<TContent, TData> ForMessage<TContent>();
    }
}