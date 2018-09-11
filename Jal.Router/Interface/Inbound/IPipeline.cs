using Jal.Router.Model;
using System;

namespace Jal.Router.Interface.Inbound
{
    public interface IPipeline
    {
        void Execute(Type[] middlewares, MessageContext context);
    }
}