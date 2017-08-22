using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ISagaRouterProvider
    {
        Saga Provide(string saganame);

        Route Provide(Saga saga, Type type, string routername);
    }
}