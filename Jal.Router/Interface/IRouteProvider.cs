using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouteProvider
    {
        Route<TContent, THandler>[] Provide<TContent, THandler>(string name);
        Route[] Provide(Type type, string name);
    }
}