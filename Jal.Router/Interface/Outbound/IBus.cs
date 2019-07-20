using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{

    public interface IBus
    {
        Task<TResult> Reply<TContent, TResult>(TContent content, Options options) where TResult : class;

        Task<TResult> Reply<TContent, TResult>(TContent content, Origin origin, Options options) where TResult : class;

        Task<TResult> Reply<TContent, TResult>(TContent content, EndPoint endpoint, Origin origin, Options options) where TResult : class;

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