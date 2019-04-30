using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Outbound
{

    public interface IBus
    {
        Task<TResult> Reply<TContent, TResult>(TContent content, Options options);

        Task<TResult> Reply<TContent, TResult>(TContent content, Origin origin, Options options);

        Task Send<TContent>(TContent content, Options options);

        Task Send<TContent>(TContent content, Origin origin, Options options);

        Task Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options);

        Task FireAndForget<TContent>(TContent content, Options options);

        Task FireAndForget<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options);

        Task FireAndForget<TContent>(TContent content, Origin origin, Options options);

        Task Publish<TContent>(TContent content, Options options);

        Task Publish<TContent>(TContent content, Origin origin, Options options);

        Task Publish<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options);
    }
}