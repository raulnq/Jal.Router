using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointOptionBuilder
    {
        IOnEndPointOptionBuilder UseMiddleware(Action<IEndpointMiddlewareBuilder> action);

        IOnEndPointOptionBuilder OnError(Action<IOnEndPointErrorBuilder> action);

        IOnEndPointOptionBuilder OnEntry(Action<IOnEndPointEntryBuilder> action);

        IOnEndPointOptionBuilder OnExit(Action<IOnEndPointExitBuilder> action);
    }
}