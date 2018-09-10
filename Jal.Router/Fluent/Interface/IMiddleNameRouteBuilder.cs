namespace Jal.Router.Fluent.Interface
{
    public interface IMiddleNameRouteBuilder<THandler, out TData>
    {
        IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>();
    }
}