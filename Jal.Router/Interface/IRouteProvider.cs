using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouteProvider
    {
        Route<TBody, TConsumer>[] Provide<TBody, TConsumer>(string name);
        Route[] Provide(Type bodytype, string name);
    }
}