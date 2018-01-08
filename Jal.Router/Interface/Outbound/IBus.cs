using Jal.Router.Model;

namespace Jal.Router.Interface.Outbound
{
    public interface IBus
    {
        TResult Reply<TContent, TResult>(TContent content, Options options);

        TResult Reply<TContent, TResult>(TContent content, Origin origin, Options options);

        void Send<TContent>(TContent content, Options options);

        void Send<TContent>(TContent content, Origin origin, Options options);

        void Send<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options);

        void FireAndForget<TContent>(TContent content, Options options);

        void FireAndForget<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options);

        void FireAndForget<TContent>(TContent content, Origin origin, Options options);

        void Publish<TContent>(TContent content, Options options);

        void Publish<TContent>(TContent content, Origin origin, Options options);

        void Publish<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options);
    }
}