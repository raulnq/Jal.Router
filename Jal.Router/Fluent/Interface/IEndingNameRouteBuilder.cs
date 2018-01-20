namespace Jal.Router.Fluent.Interface
{
    public interface IEndingNameRouteBuilder<THandler, out TData>
    {
        IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>();
    }
}