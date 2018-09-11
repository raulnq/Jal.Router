namespace Jal.Router.Fluent.Interface
{
    public interface ILastNameRouteBuilder<THandler, out TData>
    {
        IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>();
    }
}