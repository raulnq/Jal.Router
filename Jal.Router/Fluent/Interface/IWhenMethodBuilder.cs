using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenMethodBuilder<out TBody, out THandler>
    {
        void When(Func<TBody, THandler, bool> method);

        void When(Func<TBody, THandler, MessageContext, bool> method);
    }
}