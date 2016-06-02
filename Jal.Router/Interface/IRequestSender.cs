namespace Jal.Router.Interface
{
    public interface IRequestSender<in TRequest, out TResponse> : IRequestSender
    {
        TResponse Send(TRequest request, Model.EndPoint endPoint);

        TResponse Send(TRequest request, Model.EndPoint endPoint, dynamic context);

    }

    public interface IRequestSender
    {
        
    }
}