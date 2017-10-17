using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouteProvider
    {
        Route[] Provide(Type type, string routename);

        Route[] Provide(Route[] routes, Type type, string routename);

        Saga Provide(string saganame);
    }
}