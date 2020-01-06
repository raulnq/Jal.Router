﻿using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class LastRouteBuilder<TData> : ILastRouteBuilder<TData>
    {
        private readonly Saga _saga;

        public LastRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public ILastListenerRouteBuilder<TData> RegisterHandler(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new LastNameRouteBuilder<TData>(_saga, name);

            return builder;
        }
    }
}