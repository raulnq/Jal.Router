using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouteProvider
    {
        IRequestSenderSource SenderSource { get; }

        IEndPointSource EndPointSource { get; }

        Route<TRequest, TResponse>[] Provide<TRequest, TResponse>(TRequest request, string route);
    }
}
