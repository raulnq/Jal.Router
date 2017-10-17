namespace Jal.Router.Fluent.Interface
{
    public interface IStartingRouteBuilder<out TData>
    {
        IStartingNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "");
    }
}