using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEndPointSource
    {
        EndPoint[] Get<TRequest, TResponse>(TRequest request, IRequestSender<TRequest, TResponse> sender, string route);
    }
}