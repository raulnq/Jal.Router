namespace Jal.Router.Fluent.Interface
{
    public interface IContinueNameRouteBuilder<THandler, out TData>
    {
        IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>();
    }
}