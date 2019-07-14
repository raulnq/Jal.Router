using Jal.Router.Interface.Outbound;
using System.Collections.Generic;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointEntryBuilder
    {
        void Use<TMessageHandler>(IDictionary<string, object> parameters) where TMessageHandler : IBusEntryMessageHandler;
    }
}