using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenMethodBuilderWithData<out TContent, out THandler, out TData>
    {
        void When(Func<TContent, THandler, MessageContext, TData, bool> method);
    }

    public interface IWhenMethodBuilderWithData<out TContent, out TData>
    {
        void When(Func<TContent, MessageContext, TData, bool> method);
    }
}