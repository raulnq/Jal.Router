namespace Jal.Router.Fluent.Interface
{
    public interface IMiddleNameRouteBuilder<out TData>
    {
        IHandlerBuilder<TContent, TData> ForMessage<TContent>();
    }
}