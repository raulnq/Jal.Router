namespace Jal.Router.Fluent.Interface
{
    public interface IFirstNameRouteBuilder<THandler, out TData>
    {
        IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>();
    }
}