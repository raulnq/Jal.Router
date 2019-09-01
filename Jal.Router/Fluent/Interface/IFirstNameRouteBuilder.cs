namespace Jal.Router.Fluent.Interface
{
    public interface IFirstNameRouteBuilder<out TData>
    {
        IHandlerBuilder<TContent, TData> ForMessage<TContent>();
    }
}