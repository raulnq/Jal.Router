using Jal.Router.Model;
using System;

namespace Jal.Router.Interface.Outbound
{
    public interface IPipeline
    {
        object Execute(Type[] middlewares, MessageContext message, Options options, string outboundtype, Type resulttype=null);
    }
}