using System.Collections.Generic;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointExitBuilder
    {
        void Use<TMessageHandler>(IDictionary<string, object> parameters) where TMessageHandler : IBusExitMessageHandler;
    }
}