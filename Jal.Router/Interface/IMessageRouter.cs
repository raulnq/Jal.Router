namespace Jal.Router.Interface
{
    public interface IMessageRouter
    {
        void Route<TMessage>(TMessage message);

        void Route<TMessage>(TMessage message, string route);

        void Route<TMessage>(TMessage message, dynamic context, string route);

        void Route<TMessage>(TMessage message, dynamic context);

        IMessageHandlerFactory Factory { get; }

        IMessagetRouterInterceptor Interceptor { get; set; }
    }
}