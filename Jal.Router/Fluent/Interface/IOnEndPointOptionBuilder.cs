using Jal.Router.Model;
using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointOptionBuilder
    {
        IOnEndPointOptionBuilder UseMiddleware(Action<IOutboundMiddlewareBuilder> action);

        IOnEndPointOptionBuilder OnError(Action<IOnEndPointErrorBuilder> action);

        IOnEndPointOptionBuilder OnEntry(Action<IOnEndPointEntryBuilder> action);

        IOnEndPointOptionBuilder OnExit(Action<IOnEndPointExitBuilder> action);

        IOnEndPointOptionBuilder When(Func<EndPoint, Options, Type, bool> condition);

        void With(Action<IOnEndPointWithBuilder> action);


    }
}