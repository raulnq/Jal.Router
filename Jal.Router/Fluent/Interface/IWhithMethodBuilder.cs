﻿using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhithMethodBuilder<out TBody, out THandler>
    {
        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler> method);

        IWhenMethodBuilder<TBody, THandler> With<TContext>(Action<TBody, THandler, TContext> method);
    }
}