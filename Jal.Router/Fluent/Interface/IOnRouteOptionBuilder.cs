using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRouteOptionBuilder
    {
        IOnRouteOptionBuilder OnError(Action<IOnRouteErrorBuilder> action);
        IOnRouteOptionBuilder UseMiddleware(Action<IRouteMiddlewareBuilder> action);
        IOnRouteOptionBuilder OnEntry(Action<IOnRouteEntryBuilder> action);
        IOnRouteOptionBuilder OnExit(Action<IOnRouteExitBuilder> action);
    }
}