using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouter
    {
        void Route(object message, Route route);
    }
}