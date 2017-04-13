namespace Jal.Router.Interface
{
    public interface IRouter
    {
        void Route<TContent>(TContent content, string name="");

        void Route<TContent>(TContent content, dynamic context, string name = "");

        IRouteProvider Provider { get;}
    }

    public interface IRouter<in TMessage>
    {
        IRouteProvider Provider { get; }
        void Route<TContent>(TMessage message, string name = "");
    }
}