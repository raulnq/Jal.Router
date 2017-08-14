namespace Jal.Router.Interface
{
    public interface IRouter<in TMessage>
    {
        IRouteProvider Provider { get; }
        void Route<TContent>(TMessage message, string routename = "");
    }
}