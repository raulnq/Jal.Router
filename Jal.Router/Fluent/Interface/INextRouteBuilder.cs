namespace Jal.Router.Fluent.Interface
{
    public interface INextRouteBuilder<out TData>
    {
        INextListenerRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name);
    }
}