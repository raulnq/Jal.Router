using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouteProvider
    {
        Route[] Provide(Type type);

        Route[] Provide(Route[] routes, Type type);

        Saga Provide(string saganame);
    }
}