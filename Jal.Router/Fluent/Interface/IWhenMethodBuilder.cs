using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenMethodBuilder<out TContent, out THandler>
    {
        void When(Func<TContent, THandler, bool> method);

        void When(Func<TContent, THandler, MessageContext, bool> method);
    }
}