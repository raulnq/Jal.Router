namespace Jal.Router.Fluent.Interface
{
    public interface IStartRegisterRouteBuilder<TData>
    {
        IStartNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "");
    }

    public interface IContinueRegisterRouteBuilder
    {
        INameRouteBuilder<THandler> RegisterRoute<THandler>(string name = "");
    }
}