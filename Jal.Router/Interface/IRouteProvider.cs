using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouteProvider
    {
        Route[] Provide(Type type, string name);
    }

    public interface ISagaProvider
    {
        Saga[] Provide(Type type, string saganame);

        bool IsTheFirst(Saga saga, Type type);

        Route GetFirst(Saga saga, Type type);

        Route GetContinue(Saga saga, Type type, string routername);
    }
}