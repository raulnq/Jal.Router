using System;
using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRouteEntryBuilder
    {
        void Use<TMessageHandler>(IDictionary<string, object> parameters) where TMessageHandler : IRouteEntryMessageHandler;
    }
}