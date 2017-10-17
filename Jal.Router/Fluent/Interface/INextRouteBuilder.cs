namespace Jal.Router.Fluent.Interface
{
    public interface INextRouteBuilder<out TData>
    {
        INextNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "");
    }
}