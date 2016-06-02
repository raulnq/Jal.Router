namespace Jal.Router.Interface
{
    public interface IRequestSenderSource
    {
        IRequestSender<TRequest, TResponse>[] Get<TRequest, TResponse>(TRequest request, string route);
    }
}