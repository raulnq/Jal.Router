namespace Jal.Router.Fluent.Interface
{
    public interface INextNameRouteBuilder<THandler, out TData>
    {
        IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>();
    }
}